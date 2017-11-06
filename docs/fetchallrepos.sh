#!/bin/bash
rm -rf repos/
mkdir repos
cd repos/
hg clone ssh://hg@bitbucket.org/T57/linx5-components-aws-help
hg clone ssh://hg@bitbucket.org/T57/linx5-components-compression-help
hg clone ssh://hg@bitbucket.org/T57/linx5-components-cryptography-help
hg clone ssh://hg@bitbucket.org/T57/linx5-components-database-help
hg clone ssh://hg@bitbucket.org/T57/linx5-components-email-help
hg clone ssh://hg@bitbucket.org/T57/linx5-components-encryption-help

hg clone ssh://hg@bitbucket.org/T57/linx5-components-excel-help
hg clone ssh://hg@bitbucket.org/T57/linx5-components-ftp-help
hg clone ssh://hg@bitbucket.org/T57/linx5-components-finswitch-help
hg clone ssh://hg@bitbucket.org/T57/linx5-components-graphicsmagick-help
hg clone ssh://hg@bitbucket.org/T57/linx5-components-json-help
hg clone ssh://hg@bitbucket.org/T57/linx5-components-mercurial-help
hg clone ssh://hg@bitbucket.org/T57/linx5-components-msmq-help
hg clone ssh://hg@bitbucket.org/T57/linx5-components-pastel-help
hg clone ssh://hg@bitbucket.org/T57/linx5-components-pdf-help

hg clone ssh://hg@bitbucket.org/T57/linx5-components-rabbitmq-help
hg clone ssh://hg@bitbucket.org/T57/linx5-components-sqlserverreportingservices-help
hg clone ssh://hg@bitbucket.org/T57/linx5-components-utilities-help
hg clone ssh://hg@bitbucket.org/T57/linx5-components-web-help

hg clone ssh://hg@bitbucket.org/T57/linx5-components-xero-help
hg clone ssh://hg@bitbucket.org/T57/linx5-components-xero-help

cd ..
mv repos/* .
rm -r repos/