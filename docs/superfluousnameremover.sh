#!/bin/bash
ls -d */ | while read -r line
do
    withoutPrecedingLinx=${line##linx5}
    withoutComponents=${withoutPrecedingLinx##-components-}
    emptyString=''
    nameOnly="${withoutComponents/-help/$emptyString}"
    mv $line $nameOnly
done