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