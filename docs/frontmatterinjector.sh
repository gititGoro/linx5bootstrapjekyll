#!/bin/bash
find ../docs/ -depth -exec rename 's/(.*)\/([^\/]*)/$1\/\L$2/' {} \;
find | grep "index.md" | while read -r line
do
    fileContents=$(< $line)
    noIndex=${line%%/index.md}
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
    block="${block}---"$'\n'
    fileContents="${block}${fileContents}"
    echo "$fileContents">"$line"
done

find | grep "releasenotes.md" | while read -r releaseLine
do
    fileContents=$(< $releaseLine)
    noIndex=${releaseLine%%/releasenotes.md}
    group=${noIndex##*/}
    stringWithNewreleaseLines="Group: $group"$'\n'
    block="---"$'\n'
    block="${block}layout: docs"$'\n'
    block="${block}title: releasenotes"$'\n'
    block="${block}description: Release Notes"$'\n'
    block="${block}group: ${group}"$'\n'
    block="${block}toc: true"$'\n'
    block="${block}---"$'\n'
    fileContents="${block}${fileContents}"
    echo "$fileContents">"$releaseLine"
done

find | grep "licence.md" | while read -r licenceLine
do
    fileContents=$(< $licenceLine)
    noIndex=${licenceLine%%/licence.md}
    group=${noIndex##*/}
    stringWithNewlicenceLines="Group: $group"$'\n'
    block="---"$'\n'
    block="${block}layout: docs"$'\n'
    block="${block}title: licence"$'\n'
    block="${block}description: licence"$'\n'
    block="${block}group: ${group}"$'\n'
    block="${block}toc: true"$'\n'
    block="${block}---"$'\n'
    fileContents="${block}${fileContents}"
    echo "$fileContents">"$licenceLine"
done