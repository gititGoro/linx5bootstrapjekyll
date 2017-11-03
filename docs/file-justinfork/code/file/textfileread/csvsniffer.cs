using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Twenty57.Linx.Plugin.Common;

namespace Twenty57.Linx.Components.File.TextFileRead
{
	public class CSVSniffer
	{
		public char? Delimiter { get; private set; }
		public char? TextQualifier { get; private set; }
		public int HeaderLines { get; private set; }
		public string[] ColumnHeaders { get; private set; }

		private readonly char[] delimiterRange = new char[] { ',', '|', '\t', ';', ':', '-', '.' };
		private readonly char[] textQualifierRange = new char[] { '"', '\'' };
		private const int maxInferLines = 100;

		private string[] lines;

		private CSVSniffer(string fileName, int headerLines = 0)
		{
			System.Diagnostics.Trace.Assert(headerLines >= 0, "HeaderLines must be >= 0");

			this.ColumnHeaders = new string[] { };
			this.HeaderLines = headerLines;

			this.lines = ReadLines(fileName, maxInferLines);
			if (this.lines.Count() > 0)
			{
				InferDelimiter();
				if (this.Delimiter.HasValue)
				{
					InferTextQualifier();

					if (this.HeaderLines == 0)
						InferHeaderLines();

					if (this.HeaderLines == 0)
						InferColumnsFromFirstLine();
					else
						InferColumnsFromHeaderLines();
				}
			}
		}

		public static CSVSniffer Sniff(string fileName, int headerLines = 0)
		{
			return new CSVSniffer(fileName, headerLines);
		}

		private void InferTextQualifier()
		{
			var calculator = new MaxOccurrenceCalculator();
			foreach (var line in this.lines)
			{
				foreach (var textQualifier in textQualifierRange)
				{
					var count = CountMatchingQualifiersBetweenDelimiters(line, textQualifier);
					if (count > 0)
					{
						calculator.Add(textQualifier, count);
					}
				}
			}

			TextQualifier = calculator.CharacterWithCountThatOccurredTheMost;
		}

		private void InferDelimiter()
		{
			var calculator = new MaxOccurrenceCalculator();
			foreach (var line in this.lines)
			{
				foreach (var delimiter in delimiterRange)
				{
					var delimiterCount = line.Count(f => f == delimiter);
					if (delimiterCount > 0)
					{
						calculator.Add(delimiter, delimiterCount);
					}
				}
			}

			Delimiter = calculator.CharacterWithCountThatOccurredTheMost;
		}

		private void InferHeaderLines()
		{
			if (this.lines.Count() < 2)
				return;

			var firstLineFields = SplitFieldsAndStripTextQualifier(this.lines[0]);
			var noBlankFields = firstLineFields.Where(x => x.Length == 0).Count() == 0;
			var fieldsAreUnique = firstLineFields.Distinct().Count() == firstLineFields.Count();
			double parseDouble;
			var noNumberFields = firstLineFields.Where(x => double.TryParse(x, out parseDouble)).Count() == 0;
			DateTime parseDate;
			var noDateFields = firstLineFields.Where(x => DateTime.TryParse(x, out parseDate)).Count() == 0;

			if (noBlankFields && fieldsAreUnique && noNumberFields && noDateFields)
			{
				var secondLineFields = SplitFieldsAndStripTextQualifier(this.lines[1]);
				var hasBlankFields = secondLineFields.Where(x => x.Length == 0).Count() > 0;
				var hasNumberFields = secondLineFields.Where(x => double.TryParse(x, out parseDouble)).Count() > 0;
				var hasDateFields = secondLineFields.Where(x => DateTime.TryParse(x, out parseDate)).Count() > 0;

				if (hasBlankFields || hasNumberFields || hasDateFields)
					this.HeaderLines = 1;
			}
		}

		private string[] SplitFieldsAndStripTextQualifier(string line)
		{
			var fields = line.Split(this.Delimiter.Value);
			if (this.TextQualifier.HasValue)
				return fields.Select(x => x.Replace(this.Delimiter.Value.ToString(), "")).ToArray();
			else
				return fields.ToArray();
		}

		private void InferColumnsFromHeaderLines()
		{
			var linesSplitIntoFields = this.lines.Take(this.HeaderLines).Select(x => SplitFieldsAndStripTextQualifier(x));
			var lineWithMostFields = linesSplitIntoFields.OrderByDescending(x => x.Count()).First();

			var headerList = new List<string>();
			int colCounter = 1;
			foreach (var columnName in lineWithMostFields)
			{
				string validColumnName = columnName.Trim();
				if (validColumnName == "")
					validColumnName = "Column" + colCounter;
				if (!Names.IsNameValid(validColumnName))
					validColumnName = Names.GetValidName(validColumnName);
				if (!headerList.Contains(validColumnName))
					headerList.Add(validColumnName);
				else
					headerList.Add(validColumnName + colCounter);
				colCounter++;
			}
			this.ColumnHeaders = headerList.ToArray();
		}

		private void InferColumnsFromFirstLine()
		{
			var headerList = new List<string>();
			int columnCount = this.lines[0].Split(Delimiter.Value).Count();
			for (int columnNumber = 0; columnNumber < columnCount; columnNumber++)
				headerList.Add("Column" + (columnNumber + 1));
			this.ColumnHeaders = headerList.ToArray();
		}

		private int CountMatchingQualifiersBetweenDelimiters(string line, char textQualifier)
		{
			string pattern = string.Format("^{0}[^{1}]*{0}{1}|(?:{1}){0}[^{1}]*{0}(?={1})|{1}{0}[^{1}]*{0}$",
				Regex.Escape(textQualifier.ToString()),
				Regex.Escape(this.Delimiter.Value.ToString()));
			var regex = new Regex(pattern);
			return regex.Matches(line).Count;
		}

		private class MaxOccurrenceCalculator
		{
			private Dictionary<string, int> occurrences = new Dictionary<string, int>();

			public void Add(char character, int count)
			{
				string key = character.ToString() + count.ToString();
				if (occurrences.ContainsKey(key))
					occurrences[key]++;
				else
					occurrences.Add(key, 1);
			}

			public char? CharacterWithCountThatOccurredTheMost
			{
				get
				{
					if (occurrences.Count == 0)
						return null;
					else
					{
						return occurrences.OrderByDescending(x => x.Value).First().Key[0];
					}
				}
			}
		}

		private string[] ReadLines(string path, int numberOfLines)
		{
			using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			using (var streamReader = new StreamReader(fileStream))
			{
				var lines = new List<string>();
				while (!streamReader.EndOfStream)
				{
					lines.Add(streamReader.ReadLine());

					if (lines.Count >= numberOfLines)
						break;
				}

				return lines.ToArray();
			}
		}
	}
}
