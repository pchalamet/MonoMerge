#! /bin/bash

# see https://x3ro.de/integrating-a-submodule-into-the-parent-repository/

# monomerge.sh <path> <uri> <branch>

TAB="$(printf '\t')"
TARGET_PATH=$(echo -n "$1" | sed -e 's/[\/&]/\\&/g')
CMD="git ls-files -s | sed \"s/${TAB}/${TAB}$TARGET_PATH\//\" | GIT_INDEX_FILE=\${GIT_INDEX_FILE}.new git update-index --index-info && if [ -f \${GIT_INDEX_FILE}.new ]; then mv \${GIT_INDEX_FILE}.new \${GIT_INDEX_FILE}; fi"
export FILTER_BRANCH_SQUELCH_WARNING=1

git remote rm $1 || true
git branch -D monomerge/$1 || true
git remote add --fetch $1 $2
git checkout -b monomerge/$1 $1/$3
git filter-branch -f --index-filter "$CMD" HEAD
git checkout main
git merge --allow-unrelated-histories -m "monomerge $1" monomerge/$1
git remote rm $1
git branch -D monomerge/$1
