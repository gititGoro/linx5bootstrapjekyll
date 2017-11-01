#!/bin/bash
ls -d */ | while read -r line
do
    nohelp=${line%-help/}
    echo $nohelp
    mv $line $nohelp
done