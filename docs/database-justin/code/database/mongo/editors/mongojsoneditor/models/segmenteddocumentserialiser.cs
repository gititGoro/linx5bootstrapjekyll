using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ICSharpCode.AvalonEdit.Document;

namespace Twenty57.Linx.Components.MongoDB.Editors.MongoJsonEditor.Models
{
	public class SegmentedDocumentSerialiser
	{
		private readonly Dictionary<string, Func<int, string, DocumentSegment>>
			constructorDictionary;

		private readonly Regex segmentRegex;

		public SegmentedDocumentSerialiser()
		{
			constructorDictionary = new Dictionary<string, Func<
				int, string,
				DocumentSegment
				>>
			{
				{"placeholder", (offset,text) => new PlaceholderSegment(offset, text, this)},
			};

			segmentRegex = new Regex(@"\[\[(?<class>[^\s\]]+)(?<parameters>[^\]]*?)\](?<text>[\s\S]*?)\]\]", RegexOptions.Multiline);
		}

		public void AddConstructor(string name, Func<int, string, DocumentSegment> constructor)
		{
			constructorDictionary[name.ToLower()] = constructor;
		}

		public SegmentedDocument Deserialise(string input)
		{
			var segments = new List<DocumentSegment>();
			string outputString = "";
			int lastMatchPosition = 0;
			int charactersAdded = 0;

			var doc = new SegmentedDocument(new TextDocument());
			foreach (Match match in segmentRegex.Matches(input))
			{
				outputString += input.Substring(lastMatchPosition, match.Index - lastMatchPosition);
				string className = match.Groups["class"].Value.ToLower();

				if (!constructorDictionary.ContainsKey(className))
				{
					outputString += match.Value;
					lastMatchPosition = match.Index + match.Length;
					continue;
				}

				DocumentSegment segment = constructorDictionary[className](match.Index + charactersAdded, match.Groups["text"].Value);

				charactersAdded += segment.Text.Length - match.Length;

				segments.Add(segment);

				outputString += segment.Text;
				lastMatchPosition = match.Index + match.Length;
			}

			outputString += input.Substring(lastMatchPosition);
			doc.Load(outputString, segments);
			return doc;
		}
	}
}