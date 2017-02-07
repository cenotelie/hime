"""
API for the serialization of pretty-printed XML files
"""

__author__ = "Laurent Wouters <lwouters@xowl.org>"
__copyright__ = "Copyright 2014"
__license__ = "LGPL v3+"


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
