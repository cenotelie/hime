"""
API for manipulating Maven pom.xml files
"""

__author__ = "Laurent Wouters <lwouters@cenotelie.fr>"
__copyright__ = "Copyright 2014"
__license__ = "LGPL v3+"


import sys  # System
import xml.dom.minidom  # Minimal XML

# encoding of the XML files
XML_ENCODING = "UTF-8"
# identation string
XML_IDENT = "    "
# new line string
XML_NEWLINE = "\n"


def output(document, file):
    """
    Outputs the XML document in the given file with pretty printing
    :param document: The XML document to serialize
    :param file: The file to output to
    :return: None
    """
    document.normalize()
    content = XML_NEWLINE.join(
        [line for line in document.toprettyxml(XML_IDENT, XML_NEWLINE, XML_ENCODING).split(XML_NEWLINE) if
         line.strip()])
    result = open(file, "w")
    result.write(content)
    result.close()

def update_hime_version(file, version, dev):
    pom = xml.dom.minidom.parse(file)

    group_id = pom.getElementsByTagName("groupId")[0].firstChild.data
    if "fr.cenotelie.hime" in group_id:
        pom.getElementsByTagName("version")[0].firstChild.data = version + ("-SNAPSHOT" if dev else "")

    for dependency in pom.getElementsByTagName("dependency"):
        group_id = dependency.getElementsByTagName("groupId")[0].firstChild.data
        if "fr.cenotelie.hime" in group_id:
            dependency.getElementsByTagName("version")[0].firstChild.data = version + ("-SNAPSHOT" if dev else "")

    output(pom, file)

if __name__ == "__main__":
    update_hime_version(sys.argv[1], sys.argv[2], sys.argv[3] == "dev")
