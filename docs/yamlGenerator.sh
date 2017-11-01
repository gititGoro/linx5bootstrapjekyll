#!/bin/bash
find | grep "Index.md" | while read -r line
do
    fileContents=$(< $line)
    noIndex=${line%%/Index.md}
    component=${noIndex##*/}
    noComponent=${noIndex%/*}
    feature=${noComponent##*/}
    noFeature=${noComponent%/*}
    group=${noFeature##*/}
    stringWithNewLines="Group: $group"$'\n'"Feature: $feature\n Component: $component"  

    block="---"$'\n'
    block="${block}layout: docs"$'\n'
    block="${block}title: ${component}"$'\n'
    block="${block}description: ${component}"$'\n'
    block="${block}group: ${group}"$'\n'
    block="${block}feature: ${feature}"$'\n'
    block="${block}component: ${component}"$'\n'
    block="${block}toc: true"$'\n'
    block="${block}redirect_from: docs/${group}/${feature}/${component}/index"$'\n'
    block="${block}---"$'\n'
    fileContents="${block}${fileContents}"
    echo "$fileContents">"$line"
done

find | grep "ReleaseNotes.md" | while read -r line
do
    fileContents=$(< $line)
    noIndex=${line%%/ReleaseNotes.md}
    group=${noIndex##*/}
    stringWithNewLines="Group: $group"$'\n'
    block="---"$'\n'
    block="${block}layout: docs"$'\n'
    block="${block}title: ReleaseNotes"$'\n'
    block="${block}description: Release Notes"$'\n'
    block="${block}group: ${group}"$'\n'
    block="${block}toc: true"$'\n'
    block="${block}redirect_from: docs/${group}/releasenotes"$'\n'
    block="${block}---"$'\n'
    fileContents="${block}${fileContents}"
    echo "$fileContents">"$line"
done

find | grep "Licence.md" | while read -r line
do
    fileContents=$(< $line)
    noIndex=${line%%/Licence.md}
    group=${noIndex##*/}
    stringWithNewLines="Group: $group"$'\n'
    block="---"$'\n'
    block="${block}layout: docs"$'\n'
    block="${block}title: Licence"$'\n'
    block="${block}description: Licence"$'\n'
    block="${block}group: ${group}"$'\n'
    block="${block}toc: true"$'\n'
    block="${block}redirect_from: docs/${group}/licence"$'\n'
    block="${block}---"$'\n'
    fileContents="${block}${fileContents}"
    echo "$fileContents">"$line"
done