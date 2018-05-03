using System;
using System.Globalization;
using System.Collections.Generic;
using OrionCore.ErrorManagement;

namespace OrionParser.Json
{
    public class OrionJsonObject : Dictionary<String, Object>
    {
        #region Constructors
        public OrionJsonObject(String source)
        {
            Int32 iCurrentCharacterIndex;
            ParsingState xCurrentState;

            iCurrentCharacterIndex = 0;
            xCurrentState = ParsingState.None;

            this.Initialization(ref source, ref iCurrentCharacterIndex, ref xCurrentState);
        }// OrionJsonObject()
        internal OrionJsonObject(String source, ref Int32 currentCharacterIndex, ref ParsingState currentState)
        {
            this.Initialization(ref source, ref currentCharacterIndex, ref currentState);
        }// OrionJsonObject()
        #endregion

        #region Initializations
        private void Initialization(ref String source, ref Int32 currentCharacterIndex, ref ParsingState currentState)
        {
            if (String.IsNullOrWhiteSpace(source) == false)
                this.ParseObject(ref source, ref currentCharacterIndex, ref currentState);
            else
                throw new OrionException("Source Json string can't be null;");
        }// Initialization()
        #endregion

        #region Utility procedures
        private List<Object> ParseArray(ref String source, ref Int32 currentCharacterIndex, ref ParsingState currentState)
        {
            Object objValueTemp;
            List<Object> objArray;

            objArray = new List<Object>();

            do
            {
                if (source[currentCharacterIndex] == '[')
                {
                    do
                    {
                        currentCharacterIndex++;
                        objValueTemp = this.ParseValue(ref source, ref currentCharacterIndex, ref currentState);
                        if (objValueTemp != null) objArray.Add(objValueTemp);
                    } while (source[currentCharacterIndex] != ']');
                    break;
                }

                currentCharacterIndex++;
            } while (currentCharacterIndex < source.Length);

            return objArray;
        }// ParseArray()

        private KeyValue ParseKeyValue(ref String source, ref Int32 currentCharacterIndex, ref Boolean endParsing, ref ParsingState currentState)
        {
            Boolean bEndParsing;
            Char cTemp;
            Int32 iStartPos;
            KeyValue xKeyValueTemp;

            bEndParsing = false;
            xKeyValueTemp = new KeyValue();

            iStartPos = currentCharacterIndex;
            do
            {
                cTemp = source[currentCharacterIndex];

                switch (cTemp)
                {
                    case ':':
                        currentCharacterIndex++;
                        iStartPos = currentCharacterIndex;
                        xKeyValueTemp.objValue = this.ParseValue(ref source, ref currentCharacterIndex, ref currentState);
                        currentState = ParsingState.Value;
                        return xKeyValueTemp;
                    case '\"':
                        if (currentState == ParsingState.Object)
                        {
                            iStartPos = currentCharacterIndex + 1;
                            currentState = ParsingState.Name;
                        }
                        else if (currentState == ParsingState.Name)
                        {
                            xKeyValueTemp.strName = source.Substring(iStartPos, currentCharacterIndex - iStartPos);
                            currentState = ParsingState.Object;
                        }
                        break;
                    case ',':
                        if (currentState != ParsingState.ValueString) bEndParsing = true;
                        break;
                    case '}':
                        if (currentState != ParsingState.ValueString)
                        {
                            bEndParsing = true;
                            endParsing = true;
                        }
                        break;
                }

                currentCharacterIndex++;

                if (bEndParsing == true) break;
            } while (currentCharacterIndex < source.Length);

            return xKeyValueTemp;
        }// ParseKeyValuePair()
        private void ParseObject(ref String source, ref Int32 currentCharacterIndex, ref ParsingState currentState)
        {
            Boolean bEndParsing;
            KeyValue xKeyValueTemp;

            bEndParsing = false;
            do
            {
                if (source[currentCharacterIndex] == '{')
                {
                    do
                    {
                        currentState = ParsingState.Object;

                        xKeyValueTemp = this.ParseKeyValue(ref source, ref currentCharacterIndex, ref bEndParsing, ref currentState);
                        if (String.IsNullOrWhiteSpace(xKeyValueTemp.strName) == false) this.Add(xKeyValueTemp.strName, xKeyValueTemp.objValue);
                    } while (bEndParsing == false && currentCharacterIndex < source.Length - 1);
                }
                else
                    currentCharacterIndex++;
            } while (bEndParsing == false && currentCharacterIndex < source.Length - 1);
        }// ParseObject()
        private Object ParseValue(ref String source, ref Int32 currentCharacterIndex, ref ParsingState currentState)
        {
            Boolean bEndParsing;
            Char cTemp;
            Int32 iStartPos;
            Int64 lValue;
            Double dValue;
            Object objValue;
            String strValue;

            objValue = null;

            bEndParsing = false;
            iStartPos = currentCharacterIndex;
            strValue = String.Empty;
            currentState = ParsingState.Value;
            do
            {
                cTemp = source[currentCharacterIndex];
                switch (cTemp)
                {
                    case '{':
                        if (currentState != ParsingState.ValueString)
                        {
                            strValue = String.Empty;
                            objValue = new OrionJsonObject(source, ref currentCharacterIndex, ref currentState);
                            bEndParsing = true;
                        }
                        break;
                    case '[':
                        if (currentState != ParsingState.ValueString)
                        {
                            strValue = String.Empty;
                            objValue = this.ParseArray(ref source, ref currentCharacterIndex, ref currentState);
                            bEndParsing = true;
                        }
                        break;
                    case '}':
                    case ']':
                    case ',':
                        if (currentState != ParsingState.ValueString && String.IsNullOrWhiteSpace(strValue) == true)
                        {
                            strValue = source.Substring(iStartPos, currentCharacterIndex - iStartPos);
                            bEndParsing = true;
                        }
                        break;
                    case '\"':
                        if (currentState == ParsingState.Value)
                        {
                            iStartPos = currentCharacterIndex + 1;
                            currentState = ParsingState.ValueString;
                        }
                        else
                        {
                            if (source[currentCharacterIndex - 1] != '\\')
                            {
                                strValue = source.Substring(iStartPos, currentCharacterIndex - iStartPos);
                                currentCharacterIndex++;
                                currentState = ParsingState.Value;
                                bEndParsing = true;
                            }
                        }
                        break;
                }

                if (bEndParsing == true) break;

                currentCharacterIndex++;
            } while (currentCharacterIndex < source.Length);

            strValue = strValue.Trim(' ', '\n', '\r', '\t', '\"');
            if (string.IsNullOrEmpty(strValue) == false)
            {
                switch (strValue.ToUpperInvariant())
                {
                    case "NULL":
                        objValue = null;
                        break;
                    case "TRUE":
                        objValue = true;
                        break;
                    case "FALSE":
                        objValue = false;
                        break;
                    default:
                        if (Int64.TryParse(strValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out lValue) == true)
                            objValue = lValue;
                        else if (Double.TryParse(strValue, NumberStyles.Float, CultureInfo.InvariantCulture, out dValue) == true)
                            objValue = dValue;
                        else
                            objValue = strValue;
                        break;
                }
            }

            return objValue;
        }// ParseValue()
        #endregion
    }
}
