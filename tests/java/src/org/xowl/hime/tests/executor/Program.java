/*******************************************************************************
 * Copyright (c) 2015 Laurent Wouters and others
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
 ******************************************************************************/
package org.xowl.hime.tests.executor;

import org.xowl.hime.redist.ASTNode;
import org.xowl.hime.redist.ParseError;
import org.xowl.hime.redist.ParseResult;
import org.xowl.hime.redist.TextContext;
import org.xowl.hime.redist.parsers.BaseLRParser;

import java.io.File;
import java.io.IOException;
import java.lang.reflect.Method;
import java.net.URL;
import java.net.URLClassLoader;
import java.nio.charset.Charset;
import java.nio.file.FileSystems;
import java.nio.file.Files;
import java.util.ArrayList;
import java.util.List;

/**
 * Executor of tests for the Java runtime
 *
 * @author Laurent Wouters
 */
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
     * The parser have the specified output
     */
    private static final String VERB_OUTPUTS = "outputs";

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

    /**
     * Main entry point
     *
     * @param args The command parameters
     */
    public static void main(String[] args) {
        String name = args[0];
        String verb = args[1];
        Program program = new Program();
        int code = program.execute(name, verb);
        System.exit(code);
    }

    /**
     * Reads all the text of the specified file
     *
     * @param file A file
     * @return The content of the file as a string
     */
    private String readAllText(String file) {
        try {
            byte[] bytes = Files.readAllBytes(FileSystems.getDefault().getPath(file));
            return new String(bytes, "UTF-8");
        } catch (IOException ex) {
            ex.printStackTrace();
            return "";
        }
    }

    /**
     * Executes the specified test
     *
     * @param parserName The parser's name
     * @param verb       A verb specifying the type of test
     * @return The test result
     */
    public int execute(String parserName, String verb) {
        String input = readAllText("input.txt");
        BaseLRParser parser = getParser(parserName, input);
        if (VERB_MATCHES.equals(verb))
            return testMatches(parser);
        if (VERB_NOMATCHES.equals(verb))
            return testNoMatches(parser);
        if (VERB_FAILS.equals(verb))
            return testFails(parser);
        if (VERB_OUTPUTS.equals(verb))
            return testOutputs(parser);
        return RESULT_FAILURE_VERB;
    }

    /**
     * Gets the serialized expected AST
     *
     * @return The expected AST, or null if an error occurred
     */
    private ASTNode getExpectedAST() {
        String expectedText = readAllText("expected.txt");
        BaseLRParser expectedParser = getParser("Hime.Tests.Generated.ExpectedTreeParser", expectedText);
        ParseResult result = expectedParser.parse();
        for (ParseError error : result.getErrors()) {
            System.out.println(error.toString());
            TextContext context = result.getInput().getContext(error.getPosition());
            System.out.println(context.getContent());
            System.out.println(context.getPointer());
        }
        if (result.getErrors().size() > 0)
            return null;
        return result.getRoot();
    }

    /**
     * Gets the serialized expected output
     *
     * @return The expected output lines
     */
    private List<String> getExpectedOutput() {
        try {
            return Files.readAllLines((new File("expected.txt")).toPath(), Charset.forName("UTF-8"));
        } catch (IOException ex) {
            ex.printStackTrace();
            return new ArrayList<String>();
        }
    }

    /**
     * Executes the test as a parsing test with a matching condition
     *
     * @param parser The parser to use
     * @return The test result
     */
    private int testMatches(BaseLRParser parser) {
        ASTNode expected = getExpectedAST();
        if (expected == null) {
            System.out.println("Failed to parse the expected AST");
            return RESULT_FAILURE_PARSING;
        }
        ParseResult result = parser.parse();
        for (ParseError error : result.getErrors()) {
            System.out.println(error);
            TextContext context = result.getInput().getContext(error.getPosition());
            System.out.println(context.getContent());
            System.out.println(context.getPointer());
        }
        if (!result.isSuccess()) {
            System.out.println("Failed to parse the input");
            return RESULT_FAILURE_PARSING;
        }
        if (result.getErrors().size() != 0) {
            System.out.println("Some errors while parsing the input");
            return RESULT_FAILURE_PARSING;
        }
        if (compare(expected, result.getRoot())) {
            return RESULT_SUCCESS;
        } else {
            System.out.println("Produced AST does not match the expected one");
            return RESULT_FAILURE_VERB;
        }
    }

    /**
     * Executes the test as a parsing test with a non-matching condition
     *
     * @param parser The parser to use
     * @return The test result
     */
    private int testNoMatches(BaseLRParser parser) {
        ASTNode expected = getExpectedAST();
        if (expected == null) {
            System.out.println("Failed to parse the expected AST");
            return RESULT_FAILURE_PARSING;
        }
        ParseResult result = parser.parse();
        for (ParseError error : result.getErrors()) {
            System.out.println(error);
            TextContext context = result.getInput().getContext(error.getPosition());
            System.out.println(context.getContent());
            System.out.println(context.getPointer());
        }
        if (!result.isSuccess()) {
            System.out.println("Failed to parse the input");
            return RESULT_FAILURE_PARSING;
        }
        if (result.getErrors().size() != 0) {
            System.out.println("Some errors while parsing the input");
            return RESULT_FAILURE_PARSING;
        }
        if (compare(expected, result.getRoot())) {
            System.out.println("Produced AST does not match the expected one");
            return RESULT_FAILURE_VERB;
        } else {
            return RESULT_SUCCESS;
        }
    }

    /**
     * Executes the test as a parsing test with a failing condition
     *
     * @param parser The parser to use
     * @return The test result
     */
    private int testFails(BaseLRParser parser) {
        ParseResult result = parser.parse();
        if (!result.isSuccess())
            return RESULT_SUCCESS;
        if (result.getErrors().size() != 0)
            return RESULT_SUCCESS;
        System.out.println("No error found while parsing, while some were expected");
        return RESULT_FAILURE_VERB;
    }

    /**
     * Executes the test as an output test
     *
     * @param parser The parser to use
     * @return The test result
     */
    private int testOutputs(BaseLRParser parser) {
        List<String> output = getExpectedOutput();
        ParseResult result = parser.parse();
        if (output.size() == 0 || (output.size() == 1 && output.get(0).length() == 0)) {
            if (result.isSuccess() && result.getErrors().size() == 0)
                return RESULT_SUCCESS;
            for (ParseError error : result.getErrors()) {
                System.out.println(error);
                TextContext context = result.getInput().getContext(error.getPosition());
                System.out.println(context.getContent());
                System.out.println(context.getPointer());
            }
            System.out.println("Expected an empty output but some error where found while parsing");
            return RESULT_FAILURE_VERB;
        }
        int i = 0;
        for (ParseError error : result.getErrors()) {
            String message = error.toString();
            TextContext context = result.getInput().getContext(error.getPosition());
            if (i + 2 >= output.size()) {
                System.out.println("Unexpected error:");
                System.out.println(message);
                System.out.println(context.getContent());
                System.out.println(context.getPointer());
                return RESULT_FAILURE_VERB;
            }
            if (!message.startsWith(output.get(i))) {
                System.out.println("Unexpected output: " + message);
                System.out.println("Expected prefix  : " + output.get(i));
                return RESULT_FAILURE_VERB;
            }
            if (!context.getContent().startsWith(output.get(i + 1))) {
                System.out.println("Unexpected output: " + context.getContent());
                System.out.println("Expected prefix  : " + output.get(i + 1));
                return RESULT_FAILURE_VERB;
            }
            if (!context.getPointer().startsWith(output.get(i + 2))) {
                System.out.println("Unexpected output: " + context.getPointer());
                System.out.println("Expected prefix  : " + output.get(i + 2));
                return RESULT_FAILURE_VERB;
            }
            i += 3;
        }
        if (i == output.size())
            return RESULT_SUCCESS;
        for (int j = i; j != output.size(); j++)
            System.out.println("Missing output: " + output.get(j));
        return RESULT_FAILURE_VERB;
    }

    /**
     * Compare the specified AST node to the expected node
     *
     * @param expected The expected node
     * @param node     The AST node to compare
     * @return True if the nodes match
     */
    private boolean compare(ASTNode expected, ASTNode node) {
        if (!node.getSymbol().getName().equals(expected.getValue()))
            return false;
        if (expected.getChildren().get(0).getChildren().size() > 0) {
            String test = expected.getChildren().get(0).getChildren().get(0).getValue();
            String vRef = expected.getChildren().get(0).getChildren().get(1).getValue();
            vRef = vRef.substring(1, vRef.length() - 1).replace("\\'", "'").replace("\\\\", "\\");
            String vReal = node.getValue();
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

    /**
     * Gets the parser for the specified assembly and input
     *
     * @param parserName The parser's name
     * @param input      An input for the parser
     * @return The parser
     */
    private BaseLRParser getParser(String parserName, String input) {
        loadJar("Parsers.jar");
        try {
            Class lexerClass = Class.forName(parserName.substring(0, parserName.length() - 6) + "Lexer");
            Class parserClass = Class.forName(parserName);
            Object lexer = lexerClass.getConstructor(String.class).newInstance(input);
            Object parser = parserClass.getConstructor(lexerClass).newInstance(lexer);
            return (BaseLRParser) parser;
        } catch (Exception ex) {
            ex.printStackTrace();
            return null;
        }
    }

    /**
     * Loads the specified jar file
     *
     * @param file The file to load
     * @return Whether the operation succeeded
     */
    private boolean loadJar(String file) {
        File f = new File(file);
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
