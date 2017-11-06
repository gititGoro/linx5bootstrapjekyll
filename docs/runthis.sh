#!/bin/bash
./fetchallrepos.sh
./superfluousnameremover.sh
./frontmatterinjector.sh
./yamlgenerator.sh > ../_data/nav.yml