using System;
using ScintillaNET;

namespace CleeDesk
{
    public class CleeLexer
    {
        public const int
            StyleDefault = 0,
            StyleVariable = 1,
            StyleInvoking = 2,
            StyleFN = 3,
            StyleFunctionName = 4,
            StyleString = 5,
            StyleComment = 6;

        private const int
            STATE_UNKNOWN = 0,
            STATE_VARIABLE = 1,
            STATE_INVOKING = 2,
            STATE_FUNCTION = 3,
            STATE_STRING = 4,
            STATE_COMMENT = 5;

        private string keywords;

        public CleeLexer(Scintilla inScintilla) => keywords = inScintilla.DescribeKeywordSets();

        public void Style(Scintilla scintilla, int startPos, int endPos)
        {
            startPos = scintilla.Lines[scintilla.LineFromPosition(startPos)].Position;

            int length = 0, state = STATE_UNKNOWN, index = startPos;

            Action<int, int> Highlight = (len, colorizeIndex) =>
            {
                scintilla.SetStyling(len, colorizeIndex);
            };

            scintilla.StartStyling(index);
            {
                while (index < endPos)
                {
                    char _1 = (char)scintilla.GetCharAt(index),
                        _2 = index > 1 ? (char)scintilla.GetCharAt(index + 1) : default,
                        _3 = index > 2 ? (char)scintilla.GetCharAt(index + 2) : default;

                    switch (state)
                    {
                        case STATE_UNKNOWN:
                            if (_1 == '%')
                            {
                                if (_2 == '%')
                                {
                                    Highlight(3, StyleVariable);
                                    index += 3;
                                }
                                else
                                {
                                    Highlight(1, StyleVariable);
                                    index++;
                                    state = STATE_VARIABLE;
                                    length = 0;
                                }
                            }
                            else if (_1 == '.' && _2 == '/')
                            {
                                Highlight(2, StyleDefault);
                                index += 2;
                                length = 0;
                                state = STATE_INVOKING;
                            }
                            else if (_1 == 'f' && _2 == 'n')
                            {
                                Highlight(2, StyleFN);
                                index += 2;
                                state = STATE_FUNCTION;
                            }
                            else if (_1 == '"')
                            {
                                Highlight(1, StyleString);
                                index++;
                                state = STATE_STRING;
                            }
                            else if ((_1 == 'R' || _1 == 'r') && (_2 == 'E' || _2 == 'e') && (_3 == 'M' || _3 == 'm'))
                            {
                                Highlight(3, StyleComment);
                                index += 3;
                                length = 0;
                                state = STATE_COMMENT;
                            }
                            else if (_1 == ':' && _2 == ':')
                            {
                                Highlight(2, StyleComment);
                                index += 2;
                                length = 0;
                                state = STATE_COMMENT;
                            }
                            else
                            {
                                Highlight(1, StyleDefault);
                                index++;
                            }
                            break;
                        case STATE_COMMENT:
                            if (_1 == '\n')
                            {
                                length++;
                                Highlight(length, StyleComment);
                                length = 0;
                                index++;

                                state = STATE_UNKNOWN;
                            }
                            else
                            {
                                length++;
                                index++;
                            }
                            break;
                        case STATE_VARIABLE:
                            if (_1 == '%' || _1 == '\n')
                            {
                                length++;
                                Highlight(length, StyleVariable);
                                index++;
                                length = 0;
                                state = STATE_UNKNOWN;
                            }
                            else
                            {
                                length++;
                                index++;
                            }
                            break;
                        case STATE_INVOKING:
                            if (_1 == '(' || _1 == '\n')
                            {
                                Highlight(length, StyleInvoking);
                                length = 0;
                                
                                Highlight(1, StyleDefault);
                                index++;
                                
                                state = STATE_UNKNOWN;
                            }
                            else
                            {
                                length++;
                                index++;
                            }
                            break;
                        case STATE_FUNCTION:
                            if (_1 == '(' || _1 == '\n')
                            {
                                Highlight(length, StyleInvoking);
                                length = 0;
                                
                                Highlight(1, StyleDefault);
                                index++;
                                
                                state = STATE_UNKNOWN;
                            }
                            else
                            {
                                length++;
                                index++;
                            }
                            break;
                        case STATE_STRING:
                            if (_1 == '"' || _1 == '\n')
                            {
                                length++;
                                index++;

                                Highlight(length, StyleString);
                                length = 0;

                                state = STATE_UNKNOWN;
                            }
                            else
                            {
                                length++;
                                index++;
                            }
                            break;
                    }
                }
            }
        }
    }
}