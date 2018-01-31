/*******************************************************************************
 * Copyright (c) 2018 Association Cénotélie (cenotelie.fr)
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
 ******************************************************************************/

package fr.cenotelie.hime.sdk;

import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStreamWriter;
import java.net.URL;
import java.nio.charset.Charset;
import java.util.*;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

/**
 * Contains a set of helper methods for the support of Unicode
 *
 * @author Laurent Wouters
 */
public class UnicodeHelper {
    /**
     * The URL of the latest specification of Unicode blocks
     */
    private static final String URL_UNICODE_BLOCKS = "http://www.unicode.org/Public/UCD/latest/ucd/Blocks.txt";
    /**
     * The URL of the latest specification of Unicode code points
     */
    private static final String URL_UNICODE_DATA = "http://www.unicode.org/Public/UCD/latest/ucd/UnicodeData.txt";
    /**
     * The size of buffers for loading content
     */
    private static final int BUFFER_SIZE = 8192;

    /**
     * Generates Unicode-related files
     *
     * @param args The program arguments
     */
    public static void main(String[] args) {
        try {
            Collection<UnicodeBlock> blocks = getLatestBlocks();
            Collection<UnicodeCategory> categories = getLatestCategory();
            generateBlocksDb(blocks);
            generateBlocksTests(blocks);
            generateCategoriesDb(categories);
        } catch (IOException exception) {
            exception.printStackTrace();
        }
    }

    /**
     * Gets the hexadecimal representation of an integer value
     *
     * @param value The integer value
     * @return The string representation
     */
    private static String toHexString(int value) {
        String result = Integer.toHexString(value);
        switch (result.length()) {
            case 1:
                return "000" + result;
            case 2:
                return "00" + result;
            case 3:
                return "0" + result;
            case 5:
                return "000" + result;
            case 6:
                return "00" + result;
            case 7:
                return "0" + result;
        }
        return result;
    }

    /**
     * Loads all the content from the specified input stream
     *
     * @param stream The stream to load from
     * @return The loaded content
     * @throws IOException When the reading the stream fails
     */
    private static byte[] load(InputStream stream) throws IOException {
        List<byte[]> content = new ArrayList<>();
        byte[] buffer = new byte[BUFFER_SIZE];
        int length = 0;
        int read;
        int size = 0;
        while (true) {
            read = stream.read(buffer, length, BUFFER_SIZE - length);
            if (read == -1) {
                if (length != 0) {
                    content.add(buffer);
                    size += length;
                }
                break;
            }
            length += read;
            if (length == BUFFER_SIZE) {
                content.add(buffer);
                size += BUFFER_SIZE;
                buffer = new byte[BUFFER_SIZE];
                length = 0;
            }
        }

        byte[] result = new byte[size];
        int current = 0;
        for (int i = 0; i != content.size(); i++) {
            if (i == content.size() - 1) {
                // the last buffer
                System.arraycopy(content.get(i), 0, result, current, size - current);
            } else {
                System.arraycopy(content.get(i), 0, result, current, BUFFER_SIZE);
                current += BUFFER_SIZE;
            }
        }
        return result;
    }

    /**
     * Gets the latest unicode blocks from the Unicode web site
     *
     * @return The latest unicode blocks
     * @throws IOException When an IO error occurred
     */
    public static Collection<UnicodeBlock> getLatestBlocks() throws IOException {
        URL website = new URL(URL_UNICODE_BLOCKS);
        byte[] bytes;
        try (InputStream stream = website.openStream()) {
            bytes = load(stream);
        }
        String content = new String(bytes, Charset.forName("UTF-8"));
        String[] lines = content.split("\n");
        Pattern pattern = Pattern.compile("(?<begin>[0-9A-F]+)\\.\\.(?<end>[0-9A-F]+);\\s+(?<name>(\\w|\\s|-)+)");
        Collection<UnicodeBlock> blocks = new ArrayList<>();
        for (String line : lines) {
            if (line.length() == 0)
                continue;
            if (line.startsWith("#"))
                continue;
            Matcher matcher = pattern.matcher(line);
            if (!matcher.matches())
                continue;
            int begin = Integer.parseInt(matcher.group("begin"), 16);
            int end = Integer.parseInt(matcher.group("end"), 16);
            String name = matcher.group("name");
            // filter out the Surrogate-related blocks
            if (name.contains("Surrogate"))
                continue;
            name = name.replaceAll(" ", "");
            blocks.add(new UnicodeBlock(name, begin, end));
        }
        return blocks;
    }

    /**
     * Gets the latest unicode categories from the Unicode web site
     *
     * @return The latest unicode categories
     * @throws IOException When an IO error occurred
     */
    public static Collection<UnicodeCategory> getLatestCategory() throws IOException {
        URL website = new URL(URL_UNICODE_DATA);
        byte[] bytes;
        try (InputStream stream = website.openStream()) {
            bytes = load(stream);
        }
        String content = new String(bytes, Charset.forName("UTF-8"));
        String[] lines = content.split("\n");
        Pattern pattern = Pattern.compile("(?<code>[0-9A-F]+);([^;]+);(?<cat>[^;]+);.*");
        Map<String, UnicodeCategory> categories = new HashMap<>();
        String currentName = null;
        int currentSpanBegin = -1;
        int lastCP = -1;
        for (String line : lines) {
            if (line.length() == 0)
                continue;
            Matcher matcher = pattern.matcher(line);
            if (!matcher.matches())
                continue;
            int cp = Integer.parseInt(matcher.group("code"), 16);
            String categoryName = matcher.group("cat");
            if (categoryName.equals(currentName)) {
                lastCP = cp;
            } else {
                if (currentName != null) {
                    UnicodeCategory category = categories.get(categoryName);
                    if (category == null) {
                        category = new UnicodeCategory(categoryName);
                        categories.put(categoryName, category);
                    }
                    if ((currentSpanBegin < 0xD800 || currentSpanBegin >= 0xE000) && (lastCP < 0xD800 || lastCP >= 0xE000))
                        category.addSpan(currentSpanBegin, lastCP);
                }
                currentName = categoryName;
                currentSpanBegin = cp;
                lastCP = cp;
            }
        }
        categories.get(currentName).addSpan(currentSpanBegin, lastCP);
        return categories.values();
    }

    /**
     * Generates the code for the Unicode blocks data
     *
     * @param blocks The unicode blocks
     * @throws IOException When an IO error occurred
     */
    private static void generateBlocksDb(Collection<UnicodeBlock> blocks) throws IOException {
        try (FileOutputStream stream = new FileOutputStream("UnicodeBlocks.java")) {
            OutputStreamWriter writer = new OutputStreamWriter(stream, Charset.forName("UTF-8"));

            writer.write("/**\n");
            writer.write(" * WARNING: this file has been generated by\n");
            writer.write(" * Hime Parser Generator\n");
            writer.write(" */\n");
            writer.write("\n");
            writer.write("package fr.cenotelie.hime.sdk;\n");
            writer.write("\n");
            writer.write("import java.util.HashMap;\n");
            writer.write("import java.util.Map;\n");
            writer.write("\n");
            writer.write("/**\n");
            writer.write(" * Contains the supported Unicode blocks\n");
            writer.write(" */\n");
            writer.write("public class UnicodeBlocks {\n");
            for (UnicodeBlock block : blocks) {
                String javaName = block.getName().replaceAll("-", "");
                writer.write("    /**\n");
                writer.write("     * The Unicode block " + block.getName() + "\n");
                writer.write("     */\n");
                writer.write("    public static final UnicodeBlock BLOCK_" + javaName + " = new UnicodeBlock(\"" + block.getName() + "\", 0x" + toHexString(block.getBegin()) + ", 0x" + toHexString(block.getEnd()) + ");\n");
            }
            writer.write("\n");
            writer.write("    /**\n");
            writer.write("     * The database of Unicode blocks accessible by names\n");
            writer.write("     */\n");
            writer.write("    private static final Map<String, UnicodeBlock> db = new HashMap<>();\n");
            writer.write("    static {\n");
            for (UnicodeBlock block : blocks) {
                String javaName = block.getName().replaceAll("-", "");
                writer.write("        db.put(\"" + block.getName() + "\", BLOCK_" + javaName + ");\n");
            }
            writer.write("    }\n");

            writer.write("    /**\n");
            writer.write("     * Gets the block with the given name\n");
            writer.write("     *\n");
            writer.write("     * @param name A Unicode block name\n");
            writer.write("     * @return The corresponding block, or null if it does not exists\n");
            writer.write("     */\n");
            writer.write("    public static UnicodeBlock getBlock(String name) {\n");
            writer.write("        return db.get(name);\n");
            writer.write("    }\n");
            writer.write("}\n");
            writer.flush();
        }
    }

    /**
     * Generates the parsing tests for the unicode blocks
     *
     * @param blocks The unicode blocks
     * @throws IOException When an IO error occurred
     */
    private static void generateBlocksTests(Collection<UnicodeBlock> blocks) throws IOException {
        try (FileOutputStream stream = new FileOutputStream("UnicodeBlocks.suite")) {
            OutputStreamWriter writer = new OutputStreamWriter(stream, Charset.forName("UTF-8"));
            writer.write("fixture UnicodeBlocks\n");
            for (UnicodeBlock block : blocks) {
                String javaName = block.getName().replaceAll("-", "");

                writer.write("\n");
                writer.write("test Test_UnicodeBlock_" + javaName + "_LeftBound:\n");
                writer.write("\tgrammar Test_UnicodeBlock_" + javaName + "_LeftBound { options {Axiom=\"e\";} terminals {X->ub{" + block.getName() + "};} rules { e->X; } }\n");
                writer.write("\tparser LALR1\n");
                writer.write("\ton \"\\u" + toHexString(block.getBegin()) + "\"\n");
                writer.write("\tyields e(X='\\u" + toHexString(block.getBegin()) + "')\n");

                writer.write("\n");
                writer.write("test Test_UnicodeBlock_" + javaName + "_RightBound:\n");
                writer.write("\tgrammar Test_UnicodeBlock_" + javaName + "_RightBound { options {Axiom=\"e\";} terminals {X->ub{" + block.getName() + "};} rules { e->X; } }\n");
                writer.write("\tparser LALR1\n");
                writer.write("\ton \"\\u" + toHexString(block.getEnd()) + "\"\n");
                writer.write("\tyields e(X='\\u" + toHexString(block.getEnd()) + "')\n");
            }
            writer.flush();
        }
    }

    /**
     * Generates the code for the Unicode categories data
     *
     * @param categories The unicode categories
     * @throws IOException When an IO error occurred
     */
    private static void generateCategoriesDb(Collection<UnicodeCategory> categories) throws IOException {
        Map<String, Collection<UnicodeCategory>> aggregated = new HashMap<>();
        for (UnicodeCategory category : categories) {
            String aggregator = Character.toString(category.getName().charAt(0));
            aggregated.computeIfAbsent(aggregator, k -> new ArrayList<>()).add(category);
        }

        try (FileOutputStream stream = new FileOutputStream("UnicodeCategories.java")) {
            OutputStreamWriter writer = new OutputStreamWriter(stream, Charset.forName("UTF-8"));

            writer.write("/**\n");
            writer.write(" * WARNING: this file has been generated by\n");
            writer.write(" * Hime Parser Generator\n");
            writer.write(" */\n");
            writer.write("\n");
            writer.write("package fr.cenotelie.hime.sdk;\n");
            writer.write("\n");
            writer.write("import java.util.HashMap;\n");
            writer.write("import java.util.Map;\n");
            writer.write("\n");
            writer.write("/**\n");
            writer.write(" * Contains the supported Unicode categories\n");
            writer.write(" */\n");
            writer.write("public class UnicodeCategories {\n");
            for (UnicodeCategory category : categories) {
                writer.write("    /**\n");
                writer.write("     * The Unicode category " + category.getName() + "\n");
                writer.write("     */\n");
                writer.write("    private static UnicodeCategory category" + category.getName() + " = null;\n");
                writer.write("    /**\n");
                writer.write("     * Gets the Unicode category " + category.getName() + "\n");
                writer.write("     *\n");
                writer.write("     * @return The Unicode category " + category.getName() + "\n");
                writer.write("     */\n");
                writer.write("    public static UnicodeCategory get" + category.getName() + "() {\n");
                writer.write("        if (category" + category.getName() + " != null)\n");
                writer.write("            return category" + category.getName() + ";\n");
                writer.write("        category" + category.getName() + " = new UnicodeCategory(\"" + category.getName() + "\");\n");
                for (UnicodeSpan span : category.getSpans()) {
                    writer.write("        category" + category.getName() + ".addSpan(0x" + toHexString(span.getBegin()) + ", 0x" + toHexString(span.getEnd()) + ");\n");
                }
                writer.write("        return category" + category.getName() + ";\n");
                writer.write("    }\n");
            }
            for (Map.Entry<String, Collection<UnicodeCategory>> entry : aggregated.entrySet()) {
                writer.write("    /**\n");
                writer.write("     * The Unicode category " + entry.getKey() + "\n");
                writer.write("     */\n");
                writer.write("    private static UnicodeCategory category" + entry.getKey() + " = null;\n");
                writer.write("    /**\n");
                writer.write("     * Gets the Unicode category " + entry.getKey() + "\n");
                writer.write("     *\n");
                writer.write("     * @return The Unicode category " + entry.getKey() + "\n");
                writer.write("     */\n");
                writer.write("    public static UnicodeCategory get" + entry.getKey() + "() {\n");
                writer.write("        if (category" + entry.getKey() + " != null)\n");
                writer.write("            return category" + entry.getKey() + ";\n");
                writer.write("        category" + entry.getKey() + " = new UnicodeCategory(\"" + entry.getKey() + "\");\n");
                for (UnicodeCategory category : entry.getValue()) {
                    writer.write("        category" + entry.getKey() + ".aggregate(get" + category.getName() + "());\n");
                }
                writer.write("        return category" + entry.getKey() + ";\n");
                writer.write("    }\n");
            }
            writer.write("\n");
            writer.write("    /**\n");
            writer.write("     * The database of Unicode categories accessible by names\n");
            writer.write("     */\n");
            writer.write("    private static Map<String, UnicodeCategory> db = null;\n");
            writer.write("    /**\n");
            writer.write("     * Gets the category with the given name\n");
            writer.write("     *\n");
            writer.write("     * @param name A Unicode category name\n");
            writer.write("     * @return The corresponding category, or null if it does not exists\n");
            writer.write("     */\n");
            writer.write("    public static UnicodeCategory getCategory(String name) {\n");
            writer.write("        if (db != null)\n");
            writer.write("            return db.get(name);\n");
            writer.write("        db = new HashMap<>();\n");
            for (UnicodeCategory category : categories)
                writer.write("        db.put(\"" + category.getName() + "\", get" + category.getName() + "());\n");
            for (String category : aggregated.keySet())
                writer.write("        db.put(\"" + category + "\", get" + category + "());\n");
            writer.write("        return db.get(name);\n");
            writer.write("    }\n");
            writer.write("}\n");
            writer.flush();
        }
    }
}
