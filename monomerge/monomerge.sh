#! /bin/bash

# see https://x3ro.de/integrating-a-submodule-into-the-parent-repository/
TAB="$(printf '\t')"
TARGET_PATH=$(echo -n "$1" | sed -e 's/[\/&]/\\&/g')
CMD="git ls-files -s | sed \"s/${TAB}/${TAB}$TARGET_PATH\//\" | GIT_INDEX_FILE=\${GIT_INDEX_FILE}.new git update-index --index-info && if [ -f \${GIT_INDEX_FILE}.new ]; then mv \${GIT_INDEX_FILE}.new \${GIT_INDEX_FILE}; fi"

git ls-files -s | sed "s/${TAB}/${TAB}$TARGET_PATH\//"

export FILTER_BRANCH_SQUELCH_WARNING=1
git filter-branch -f --index-filter "$CMD" HEAD
