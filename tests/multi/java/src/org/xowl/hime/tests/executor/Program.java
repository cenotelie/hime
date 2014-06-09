/**********************************************************************
 * Copyright (c) 2014 Laurent Wouters and others
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as
 * published by the Free Software Foundation, either version 3
 * of the License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General
 * Public License along with this program.
 * If not, see <http://www.gnu.org/licenses/>.
 *
 * Contributors:
 *     Laurent Wouters - lwouters@xowl.org
 **********************************************************************/
package org.xowl.hime.tests.executor;

import org.apache.xerces.parsers.DOMParser;
import org.w3c.dom.Document;
import org.w3c.dom.Node;
import org.xowl.hime.redist.ASTNode;
import org.xowl.hime.redist.ParseResult;
import org.xowl.hime.redist.parsers.IParser;

import java.io.File;
import java.lang.reflect.Method;
import java.net.URL;
import java.net.URLClassLoader;

public class Program {
    private static final String VERB_MATCHES = "matches";
    private static final String VERB_NOMATCHES = "nomatches";
    private static final String VERB_FAILS = "fails";

    /**
     * The test was successful
     */
    private static final int RESULT_SUCCESS = 0;
    /**
     * The test failed in the end
     */
    private static final int RESULT_FAILURE_VERB = 1;
    /**
     * The test failed in its parsing phase
     */
    private static final int RESULT_FAILURE_PARSING = 2;

    public static void main(String[] args) {
        String pathToAssembly = getValue(args[0]);
        String name = getValue(args[1]);
        String input = getValue(args[2]);
        String verb = args[3];
        String pathToExpected = null;
        if (VERB_FAILS.equals(verb))
            pathToExpected = getValue(args[4]);
        Program program = new Program();
        int code = program.execute(pathToAssembly, name, input, verb, pathToExpected);
        System.exit(code);
    }

    private static String getValue(String arg) {
        if (arg.startsWith("\""))
            return arg.substring(1, arg.length() - 1);
        return arg;
    }

    public int execute(String pathToAssembly, String name, String input, String verb, String pathToExpected) {
        IParser parser = getParser(pathToAssembly, name, input);
        Document expected = null;
        if (pathToExpected != null) {
            DOMParser xmlParser = new DOMParser();
            try {
                xmlParser.parse(pathToExpected);
                expected = xmlParser.getDocument();
            } catch (Exception ex) {
            }
        }
        if (VERB_MATCHES.equals(verb))
            return testMatches(parser, expected);
        if (VERB_NOMATCHES.equals(verb))
            return testNoMatches(parser, expected);
        if (VERB_FAILS.equals(verb))
            return testFails(parser);
        return RESULT_FAILURE_VERB;
    }

    private int testMatches(IParser parser, Document expected) {
        ParseResult result = parser.parse();
        if (!result.isSuccess())
            return RESULT_FAILURE_PARSING;
        if (result.getErrors().size() != 0)
            return RESULT_FAILURE_PARSING;
        boolean comparison = compare(expected.getChildNodes().item(1), result.getRoot());
        return comparison ? RESULT_SUCCESS : RESULT_FAILURE_VERB;
    }

    private int testNoMatches(IParser parser, Document expected) {
        ParseResult result = parser.parse();
        if (!result.isSuccess())
            return RESULT_FAILURE_PARSING;
        if (result.getErrors().size() != 0)
            return RESULT_FAILURE_PARSING;
        boolean comparison = compare(expected.getChildNodes().item(1), result.getRoot());
        return comparison ? RESULT_FAILURE_VERB : RESULT_SUCCESS;
    }

    private int testFails(IParser parser) {
        ParseResult result = parser.parse();
        if (!result.isSuccess())
            return RESULT_SUCCESS;
        if (result.getErrors().size() != 0)
            return RESULT_SUCCESS;
        return RESULT_FAILURE_VERB;
    }

    private boolean compare(Node expected, ASTNode node) {
        if (!node.getSymbol().getName().equals(expected.getLocalName()))
            return false;
        if (expected.hasAttributes()) {
            String test = expected.getAttributes().getNamedItem("test").getNodeValue();
            String vRef = expected.getAttributes().getNamedItem("value").getNodeValue();
            String vReal = node.getSymbol().getValue();
            if (VERB_MATCHES.equals(test) && !vReal.equals(vRef))
                return false;
            if (VERB_NOMATCHES.equals(test) && vReal.equals(vRef))
                return false;
        }
        if (node.getChildren().size() != expected.getChildNodes().getLength())
            return false;
        for (int i = 0; i != node.getChildren().size(); i++)
            if (!compare(expected.getChildNodes().item(i), node.getChildren().get(i)))
                return false;
        return true;
    }

    private IParser getParser(String pathToAssembly, String name, String input) {
        loadJar(pathToAssembly);
        try {
            Class lexerClass = Class.forName("org.xowl.hime.tests.generated." + name + "Lexer");
            Class parserClass = Class.forName("org.xowl.hime.tests.generated." + name + "Parser");
            Object lexer = lexerClass.getConstructor(String.class).newInstance(input);
            Object parser = parserClass.getConstructor(lexerClass).newInstance(lexer);
            return (IParser)parser;
        } catch (Exception ex) {
            return null;
        }
    }

    private boolean loadJar(String pathToAssembly) {
        File f = new File(pathToAssembly);
        if (!f.exists())
            return false;
        try {
            URL url = f.toURI().toURL();
            Method addURL = URLClassLoader.class.getDeclaredMethod("addURL", new Class[]{URL.class});
            addURL.setAccessible(true);
            addURL.invoke(ClassLoader.getSystemClassLoader(), url);
        } catch (Exception ex) {
            return false;
        }
        return true;
    }
}
