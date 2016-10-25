using System;
using System.IO;
using Lucene.Net.Analysis;

namespace Manulife.SearchEngine.LuceneNet.Analyzers
{
    public class CJKTokenizer : Tokenizer
    {
        //~ Static fields/initializers ---------------------------------------------

        /**//** Max word length */
        private static int MAX_WORD_LEN = 255;

        /**//** buffer size: */
        private static int IO_BUFFER_SIZE = 256;

        //~ Instance fields --------------------------------------------------------

        /**//** word offset, used to imply which character(in ) is parsed */
        private int offset = 0;

        /**//** the index used only for ioBuffer */
        private int bufferIndex = 0;

        /**//** data length */
        private int dataLen = 0;

        /**//**
         * character buffer, store the characters which are used to compose <br>
         * the returned Token
         */
        private char[] buffer = new char[MAX_WORD_LEN];

        /**//**
         * I/O buffer, used to store the content of the input(one of the <br>
         * members of Tokenizer)
         */
        private char[] ioBuffer = new char[IO_BUFFER_SIZE];

        /**//** word type: single=>ASCII double=>non-ASCII word=>default */
        private string tokenType = "word";

        /**//**
         * tag: previous character is a cached double-byte character "C1C2C3C4"
         * ----(set the C1 isTokened) C1C2 "C2C3C4" ----(set the C2 isTokened)
         * C1C2 C2C3 "C3C4" ----(set the C3 isTokened) "C1C2 C2C3 C3C4"
         */
        private bool preIsTokened = false;

        //~ Constructors -----------------------------------------------------------

        /**//**
         * Construct a token stream processing the given input.
         *
         * @param in I/O reader
         */
        public CJKTokenizer(TextReader reader)
        {
            input = reader;
        }

        //~ Methods ----------------------------------------------------------------

        /**//**
         * Returns the next token in the stream, or null at EOS.
         * See http://java.sun.com/j2se/1.3/docs/api/java/lang/Character.UnicodeBlock.html
         * for detail.
         *
         * @return Token
         *
         * @throws java.io.IOException - throw IOException when read error <br>
         *         hanppened in the InputStream
         *
         */
        public override Token Next()
        {
            /**//** how many character(s) has been stored in buffer */
            int length = 0;

            /**//** the position used to create Token */
            int start = offset;

            while (true)
            {
                /**//** current charactor */
                char c;


                offset++;

                /**//*
                 if (bufferIndex >= dataLen)
                 {
                        dataLen = input.read(ioBuffer); //Java中read读到最后不会出错，但.Net会，
                        bufferIndex = 0;
                 }
                 */

                if (bufferIndex >= dataLen)
                {
                    if (dataLen == 0 || dataLen >= ioBuffer.Length)//Java中read读到最后不会出错，但.Net会，所以此处是为了拦截异常
                    {
                        dataLen = input.Read(ioBuffer, 0, ioBuffer.Length);
                        bufferIndex = 0;
                    }
                    else
                    {
                        dataLen = 0;
                    }
                }

                if (dataLen == 0)
                {
                    if (length > 0)
                    {
                        if (preIsTokened == true)
                        {
                            length = 0;
                            preIsTokened = false;
                        }

                        break;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    //get current character
                    c = ioBuffer[bufferIndex++];
                }

                //if the current character is ASCII or Extend ASCII
                if (IsAscii(c) || IsHALFWIDTH_AND_FULLWIDTH_FORMS(c))
                {
                    if (IsHALFWIDTH_AND_FULLWIDTH_FORMS(c))
                    {
                        /**//** convert HALFWIDTH_AND_FULLWIDTH_FORMS to BASIC_LATIN */
                        int i = (int)c;
                        i = i - 65248;
                        c = (char)i;
                    }
                    //if the current character is a letter or "_" "+" "##region if the current character is a letter or "_" "+" "#

                    // if the current character is a letter or "_" "+" "#"
                    if (char.IsLetterOrDigit(c) || ((c == '_') || (c == '+') || (c == '#')))
                    {

                        if (length == 0)
                        {
                            // "javaC1C2C3C4linux" <br>
                            //      ^--: the current character begin to token the ASCII
                            // letter
                            start = offset - 1;
                        }
                        else if (tokenType == "double")
                        {
                            // "javaC1C2C3C4linux" <br>
                            //              ^--: the previous non-ASCII
                            // : the current character
                            offset--;
                            bufferIndex--;
                            tokenType = "single";

                            if (preIsTokened == true)
                            {
                                // there is only one non-ASCII has been stored
                                length = 0;
                                preIsTokened = false;

                                break;
                            }
                            else
                            {
                                break;
                            }
                        }

                        // store the LowerCase(c) in the buffer
                        buffer[length++] = char.ToLower(c);
                        tokenType = "single";
                        // break the procedure if buffer overflowed!
                        if (length == MAX_WORD_LEN)
                        {
                            break;
                        }
                    }
                    else if (length > 0)
                    {
                        if (preIsTokened == true)
                        {
                            length = 0;
                            preIsTokened = false;
                        }
                        else
                        {
                            break;
                        }
                    }

                }
                else
                {
                    // non-ASCII letter, eg."C1C2C3C4"#region // non-ASCII letter, eg."C1C2C3C4"

                    // non-ASCII letter, eg."C1C2C3C4"
                    if (char.IsLetter(c))
                    {
                        if (length == 0)
                        {
                            start = offset - 1;
                            buffer[length++] = c;
                            tokenType = "double";
                        }
                        else
                        {
                            if (tokenType == "single")
                            {
                                offset--;
                                bufferIndex--;

                                //return the previous ASCII characters
                                break;
                            }
                            else
                            {
                                buffer[length++] = c;
                                tokenType = "double";

                                if (length == 2)
                                {
                                    offset--;
                                    bufferIndex--;
                                    preIsTokened = true;

                                    break;
                                }
                            }
                        }
                    }
                    else if (length > 0)
                    {
                        if (preIsTokened == true)
                        {
                            // empty the buffer
                            length = 0;
                            preIsTokened = false;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }

            return new Token(new String(buffer, 0, length), start, start + length,
                tokenType
                );
        }

        public bool IsAscii(char c)
        {
            return c < 256 && c >= 0;
        }

        public bool IsHALFWIDTH_AND_FULLWIDTH_FORMS(char c)
        {
            return c <= 0xFFEF && c >= 0xFF00;
        }
    }
}
