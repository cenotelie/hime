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

import org.xowl.hime.redist.ASTNode;
import org.xowl.hime.redist.ParseError;
import org.xowl.hime.redist.ParseResult;
import org.xowl.hime.redist.parsers.IParser;

import java.io.File;
import java.io.IOException;
import java.lang.reflect.Method;
import java.net.URL;
import java.net.URLClassLoader;
import java.nio.file.FileSystems;
import java.nio.file.Files;

public class Program {
    /**
     * The parser must produce an AST that matches the expected one
     */
    private static final String VERB_MATCHES = "matches";
    /**
     * The parser must produce an AST that do NOT match the expected one
     */
    private static final String VERB_NOMATCHES = "nomatches";
    /**
     * The parser must fail
     */
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
        String name = args[0];
        String verb = args[1];
        Program program = new Program();
        int code = program.execute(name, verb);
        System.exit(code);
    }

    private String readAllText(String file) {
        try {
            byte[] bytes = Files.readAllBytes(FileSystems.getDefault().getPath(file));
            return new String(bytes, "UTF-8");
        } catch (IOException ex) {
            ex.printStackTrace();
            return "";
        }
    }

    public int execute(String name, String verb) {
        String input = readAllText("input.txt");
        IParser parser = getParser(name, input);
        ASTNode expected = null;
        if (!VERB_FAILS.equals(verb)) {
            String expectedText = readAllText("expected.txt");
            IParser expectedParser = getParser("Hime.Tests.Generated.ExpectedTreeParser", expectedText);
            ParseResult result = expectedParser.parse();
            for (ParseError error : result.getErrors()) {
                System.out.println(error.toString());
                String[] context = result.getInput().getContext(error.getPosition());
                System.out.println(context[0]);
                System.out.println(context[1]);
            }
            if (result.getErrors().size() > 0)
                return RESULT_FAILURE_PARSING;
            expected = result.getRoot();
        }
        if (VERB_MATCHES.equals(verb))
            return testMatches(parser, expected);
        if (VERB_NOMATCHES.equals(verb))
            return testNoMatches(parser, expected);
        if (VERB_FAILS.equals(verb))
            return testFails(parser);
        return RESULT_FAILURE_VERB;
    }

    private int testMatches(IParser parser, ASTNode expected) {
        ParseResult result = parser.parse();
        if (!result.isSuccess())
            return RESULT_FAILURE_PARSING;
        if (result.getErrors().size() != 0)
            return RESULT_FAILURE_PARSING;
        boolean comparison = compare(expected, result.getRoot());
        return comparison ? RESULT_SUCCESS : RESULT_FAILURE_VERB;
    }

    private int testNoMatches(IParser parser, ASTNode expected) {
        ParseResult result = parser.parse();
        if (!result.isSuccess())
            return RESULT_FAILURE_PARSING;
        if (result.getErrors().size() != 0)
            return RESULT_FAILURE_PARSING;
        boolean comparison = compare(expected, result.getRoot());
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

    private boolean compare(ASTNode expected, ASTNode node) {
        if (!node.getSymbol().getName().equals(expected.getSymbol().getValue()))
            return false;
        if (expected.getChildren().get(0).getChildren().size() > 0) {
            String test = expected.getChildren().get(0).getChildren().get(0).getSymbol().getValue();
            String vRef = expected.getChildren().get(0).getChildren().get(1).getSymbol().getValue();
            vRef = vRef.substring(1, vRef.length() - 1).replace("\\'", "'").replace("\\\\", "\\");
            String vReal = node.getSymbol().getValue();
            if ("=".equals(test) && !vRef.equals(vReal))
                return false;
            if ("!=".equals(test) && vRef.equals(vReal))
                return false;
        }
        if (node.getChildren().size() != expected.getChildren().get(1).getChildren().size())
            return false;
        for (int i = 0; i != node.getChildren().size(); i++)
            if (!compare(expected.getChildren().get(1).getChildren().get(i), node.getChildren().get(i)))
                return false;
        return true;
    }

    private IParser getParser(String name, String input) {
        loadJar("Parsers.jar");
        try {
            Class lexerClass = Class.forName(name.substring(0, name.length() - 6) + "Lexer");
            Class parserClass = Class.forName(name);
            Object lexer = lexerClass.getConstructor(String.class).newInstance(input);
            Object parser = parserClass.getConstructor(lexerClass).newInstance(lexer);
            return (IParser)parser;
        } catch (Exception ex) {
            ex.printStackTrace();
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
            ex.printStackTrace();
            return false;
        }
        return true;
    }
}
