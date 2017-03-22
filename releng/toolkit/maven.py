"""
API for manipulating Maven pom.xml files
"""

__author__ = "Laurent Wouters <lwouters@cenotelie.fr>"
__copyright__ = "Copyright 2014"
__license__ = "LGPL v3+"


import sys  # System
import xml.dom.minidom  # Minimal XML

import xmlutils  # XML utilities


def update_hime_version(file, version, dev):
    pom = xml.dom.minidom.parse(file)

    group_id = pom.getElementsByTagName("groupId")[0].firstChild.data
    if "org.xowl.hime" in group_id:
        pom.getElementsByTagName("version")[0].firstChild.data = version + ("-SNAPSHOT" if dev else "")

    for dependency in pom.getElementsByTagName("dependency"):
        group_id = dependency.getElementsByTagName("groupId")[0].firstChild.data
        if "org.xowl.hime" in group_id:
            dependency.getElementsByTagName("version")[0].firstChild.data = version + ("-SNAPSHOT" if dev else "")

    xmlutils.output(pom, file)

if __name__ == "__main__":
    update_hime_version(sys.argv[1], sys.argv[2], sys.argv[3] == "dev")
