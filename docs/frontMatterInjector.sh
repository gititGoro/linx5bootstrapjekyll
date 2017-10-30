#!/bin/bash
find | grep "Index.md" | while read -r line
do
    noIndex=${line%%/Index.md}
    component=${noIndex##*/}
    noComponent=${noIndex%/*}
    feature=${noComponent##*/}
    noFeature=${noComponent%/*}
    group=${noFeature##*/}
    echo "Group: $group, Feature: $feature, Component: $component"  
    #TODO: get group as well
done


