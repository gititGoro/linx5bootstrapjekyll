#!/bin/bash
rm nav.yml
ls -d */ | while read -r groupDirectory
do
    groupTitle=${groupDirectory%%/}
    echo "- title: $groupTitle"
    cd $groupDirectory
    if [[ $(ls *.md) ]]; then #top level markdown files
       echo "  pages: "
       ls *.md | while read -r topLevelPage
       do
        noExtension=${topLevelPage%%.md}
       echo "    - title: $noExtension" 
       done
    fi

    if [[ $(ls -d */) ]]; then #features
       echo "  features:"
        ls -d */ | while read -r featureDirectory
        do
             featureString=${featureDirectory%%/} 
            echo "    - feature: $featureString" 
             cd $featureDirectory
                  if [[ $(ls -d */) ]]; then #components
                       echo "      components:"
                           ls -d */ | while read -r componentDirectory
                           do
                                componentString=${componentDirectory%%/} 
                               echo "        - component: $componentString"
                               echo "          pages:" 
                               echo "            - title: index"
                           done
                  fi
             cd ..
        done
    fi
    
    cd ..
done


