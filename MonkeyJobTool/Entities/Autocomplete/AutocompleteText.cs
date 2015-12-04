﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using HelloBotCommunication;
using HelloBotCore.Entities;
using CallCommandInfo = HelloBotCore.Entities.CallCommandInfo;

namespace MonkeyJobTool.Entities.Autocomplete
{
    public class AutocompleteText
    {
        private readonly RichTextBox _bindedControl;
        

        List<AutocompleteTextPart> _parts { get; set; }
        private int _currCaretPos = 0;
        private int _currCaretSelectionLength = 0;

        private int _prevCaretPos = 0;
        private int _prevCaretSelectionLength = 0;
        private bool _changedEventEnabled = true;

        public delegate void CommandFocusedDelegate(string commandText);
        public event CommandFocusedDelegate OnCommandFocused;

        public delegate void CommandBluredDelegate();
        public event CommandBluredDelegate OnCommandBlured;

        public delegate CallCommandInfo TryResolveCommandFromStringDelegate(string text);
        private TryResolveCommandFromStringDelegate _tryResolveCommandFromStringFunc;

        public AutocompleteText(RichTextBox bindedControl, TryResolveCommandFromStringDelegate tryResolveCommandFromStringFunc)
        {
            _bindedControl = bindedControl;
            _tryResolveCommandFromStringFunc = tryResolveCommandFromStringFunc;
            _bindedControl.MouseDown += (sender, args) => { changeCaretPos(); };
            _bindedControl.MouseUp += (sender, args) => { changeCaretPos(); };
            _bindedControl.TextChanged += (sender, args) =>
            {
                
                if (_changedEventEnabled)
                {
                    changeCaretPos(true);
                    textChanged();
                    ClearParts();
                    CorrectParts();

                    var focusedPart = GetFocusedPart();
                    TryResolveCommandIfRequired(focusedPart);
                    TryNotifyAboutCommandFocused(focusedPart);
                    TryNotifyAboutCommandBlured(focusedPart);
                    CheckForAvailableArgumentSuggestions(focusedPart);
                    RefreshText();
                    Log();
                }
                
            };
            _bindedControl.KeyUp += (sender, args) => { changeCaretPos(); };
            _parts = new List<AutocompleteTextPart>();
            AddEmptyCommand();
            _lastText = bindedControl.Text;
        }


        private AutocompleteTextPart GetFocusedPart()
        {
            AutocompleteTextPart toReturn = null;
            var pos = 0;
            foreach (var part in _parts)
            {
                if (_currCaretPos >= pos && _currCaretPos <= pos + part.Text.Length)
                {
                    toReturn = part;
                }
                pos += part.Text.Length;
            }

            return toReturn;
        }

        private void AddEmptyCommand()
        {
            AppendParts(new AutoCompleteCommandPart());
        }

        private void changeCaretPos(bool fromChangedTextEvent = false)
        {
            AutocompleteTextPart partInFocus = null;
            if ((_currCaretPos != _bindedControl.SelectionStart || fromChangedTextEvent) || _currCaretSelectionLength != _bindedControl.SelectionLength)
            {
                _prevCaretPos = _currCaretPos;
                _prevCaretSelectionLength = _currCaretSelectionLength;
                _currCaretPos = _bindedControl.SelectionStart;
                _currCaretSelectionLength = _bindedControl.SelectionLength;

                if (!fromChangedTextEvent)
                {
                    TryNotifyAboutCommandFocused();
                    TryNotifyAboutCommandBlured();
                }

                Console.WriteLine(_prevCaretPos + " " + _currCaretPos);
            }
        }

        private void SetBackColor(Color color)
        {
            _bindedControl.SelectionBackColor = color;
        }

        private string _lastText = string.Empty;
        private void textChanged()
        {

            if (_lastText != _bindedControl.Text)
            {
                if (_prevCaretSelectionLength != 0 && _currCaretSelectionLength == 0)
                {
                    //selected and removed

                    bool shouldInsert = _lastText.Length - _prevCaretSelectionLength != _bindedControl.Text.Length;
                    ChangePartOfText(_lastText.Substring(_prevCaretPos, _prevCaretSelectionLength), _prevCaretPos, _prevCaretPos + _prevCaretSelectionLength, true);
                    if (shouldInsert)
                    {
                        //delete parts after delete
                        ClearParts();

                        //deleted and inserted
                        Console.WriteLine("and inserted : " + _bindedControl.Text.Substring(_prevCaretPos, _currCaretPos - _prevCaretPos));
                        ChangePartOfText(_bindedControl.Text.Substring(_prevCaretPos, _currCaretPos - _prevCaretPos), _prevCaretPos, _prevCaretPos, false);
                    }
                }
                else
                {
                    if (_bindedControl.Text.Length > _lastText.Length)
                    {
                        //added new string
                        Console.WriteLine("added new part : " + _bindedControl.Text.Substring(_prevCaretPos, _currCaretPos - _prevCaretPos));
                        ChangePartOfText(_bindedControl.Text.Substring(_prevCaretPos, _currCaretPos - _prevCaretPos), _prevCaretPos, _prevCaretPos, false);
                    }
                    else
                    {
                        var correctedPrevCaretPos = _prevCaretPos;
                        if (_prevCaretPos == _currCaretPos) //del btn pressed
                        {
                            correctedPrevCaretPos++;
                        }
                        //text deleted
                        Console.WriteLine("text deleted : " + _lastText.Substring(_currCaretPos, correctedPrevCaretPos - _currCaretPos));
                        ChangePartOfText(_lastText.Substring(_currCaretPos, correctedPrevCaretPos - _currCaretPos), _currCaretPos, correctedPrevCaretPos, true);
                    }
                }
            }
            _lastText = _bindedControl.Text;
            Console.WriteLine("textChanged()");
        }
        private void TryResolveCommandIfRequired(AutocompleteTextPart focusedPart)
        {
            if (IsCommandInFocus(focusedPart))
            {
                Console.WriteLine("_tryResolveCommandFromStringFunc()");
                var command = _tryResolveCommandFromStringFunc(CommandPart.Text);
                CommandPart.Command = command;
            }
        }
        private void TryNotifyAboutCommandFocused(AutocompleteTextPart focusedPart=null)
        {
            if (IsCommandInFocus(focusedPart))
            {
                if (OnCommandFocused != null)
                    OnCommandFocused(CommandPart.Text);
            }
        }
        private void TryNotifyAboutCommandBlured(AutocompleteTextPart focusedPart=null)
        {
            if (!IsCommandInFocus(focusedPart))
            {
                if (OnCommandBlured != null)
                    OnCommandBlured();
            }
        }

        private bool IsCommandInFocus(AutocompleteTextPart focusedPart)
        {
            return (focusedPart!=null && focusedPart.Type == AutocompleteTextPartType.Command) || (_currCaretPos <= CommandPart.Text.Length && !string.IsNullOrEmpty(CommandPart.Text));
        }

        private void ChangePartOfText(string val, int fromPos, int toPos, bool deleted)
        {
            
            int currPos = 0;
            AutocompleteTextPart fromAffectedPart = null;
            AutocompleteTextPart toAffectedPart = null;
            for (int i = 0; i < _parts.Count; i++)
            {
                var part = _parts[i];
                if (fromPos >= currPos && fromPos <= currPos + part.Text.Length)
                {
                    //detect border and try to resolve part select
                    if (fromPos == toPos && fromPos == currPos + part.Text.Length &&
                        i != _parts.Count - 1 &&
                        part.Text.EndsWith(" ") && !_parts[i + 1].Text.StartsWith(" "))
                    {
                        fromAffectedPart = _parts[i + 1];
                    }
                    else
                    {
                        fromAffectedPart = part;
                    }

                }
                if (toPos >= currPos && toPos <= currPos + part.Text.Length)
                {
                    //detect border and try to resolve part select
                    if (fromPos == toPos && fromPos == currPos + part.Text.Length &&
                        i != _parts.Count - 1 &&
                        part.Text.EndsWith(" ") && !_parts[i + 1].Text.StartsWith(" "))
                    {
                        toAffectedPart = _parts[i + 1];
                    }
                    else
                    {
                        toAffectedPart = part;
                    }

                    //break;
                }

                currPos += part.Text.Length;
            }
            //one part affected
            if (fromAffectedPart.Index == toAffectedPart.Index || !deleted)
            {
                var startPos = GetStartPartPos(fromAffectedPart.Index);
                if (!deleted)
                {
                    fromAffectedPart.Text = fromAffectedPart.Text.Insert(fromPos - startPos, val);
                }
                else
                {
                    fromAffectedPart.Text = fromAffectedPart.Text.Remove(fromPos - startPos, toPos - fromPos);
                }
            }
            else
            {
                //many parts affected   

                AutocompleteTextPart changedPart = fromAffectedPart;
                do
                {
                    var startPos = GetStartPartPos(changedPart.Index);
                    bool fullRemove = startPos + changedPart.Text.Length - 1 < toPos && fromPos <= startPos;
                    if (fullRemove)
                    {
                        toPos -= changedPart.Text.Length;
                        changedPart.Delete();
                        changedPart.MarkAsDeleted = true;
                    }
                    else
                    {
                        if (changedPart.Index == fromAffectedPart.Index)
                        {
                            toPos -= (changedPart.Text.Length - (fromPos - startPos));
                            changedPart.Text = changedPart.Text.Substring(0, fromPos - startPos);
                        }
                        else if (changedPart.Index == toAffectedPart.Index)
                        {
                            changedPart.Text = changedPart.Text.Substring(toPos - startPos);
                        }
                        else
                            throw new ApplicationException("alg fault");

                    }
                } while ((changedPart = _parts.SingleOrDefault(x => x.Index == changedPart.Index + 1 && x.Index <= toAffectedPart.Index)) != null);
            }
            
            
        }

        private void CorrectParts()
        {
            if (_parts.Count > 1)
            {
                for (int i = 0; i < _parts.Count; i++)
                {

                    var part = _parts[i];
                    var nextPart = i != _parts.Count - 1 ? _parts[i + 1] : null;
                    part.Index = i;
                    //merge text parts
                    if (part.Type == AutocompleteTextPartType.PartOfText && nextPart != null && nextPart.Type==AutocompleteTextPartType.PartOfText)
                    {
                        part.Text += nextPart.Text;
                        _parts.RemoveAt(i+1);
                    }
                    
                }
            }

            if (CommandPart.Text.EndsWith(" ") && CommandPart.Command != null)
            {
                CommandPart.Text = CommandPart.Text.TrimEnd(' ');
                if (_parts.Count == 1 || (_parts.Count > 1 && _parts[1].Type != AutocompleteTextPartType.Delimeter))
                {
                    AppendParts(new AutoCompleteDelimeterPart()
                    {
                        
                        Text = " "
                    }, false);
                }
            }

            //insert new text part after command delimeter
            if (_parts.Count > 1 && _parts[1].Type == AutocompleteTextPartType.Delimeter && _parts[1].Text==" ")
            {
                if (_parts.Count == 2 /*|| (_parts.Count > 2 && _parts[2].Type != AutocompleteTextPartType.PartOfText)*/)
                {
                    AppendParts(new AutocompleteTextPart()
                    {
                        
                        Text = ""
                    });
                }
            }

            var suggPart = _parts.Last() as AutoCompleteArgumentSuggestPart;
            if (suggPart!=null)
            {
                if (suggPart.Suggest != null)
                {
                    var argToCompare = suggPart.Text.ToLower();
                    var found = suggPart.SuggestCases.FirstOrDefault(x => x.Value.ToLower() == argToCompare);

                    //if new char added but command for new text not found
                    if (found == null  &&
                        suggPart.Suggest.Value.Length <= suggPart.Text.Length //in case of delete - skip
                        )
                    {
                        var endSuggPos = GetStartPartPos(suggPart.Index)+suggPart.Text.Length;
                        if (endSuggPos == _currCaretPos) //if added to the end of sugg
                        {
                            //assume, that added char is new part of text, suggestion finished
                            var newTextPart = suggPart.Text.Substring(suggPart.Suggest.Value.Length);
                            suggPart.Text = suggPart.Text.Substring(0, suggPart.Suggest.Value.Length);
                            AppendParts(new AutocompleteTextPart()
                            {
                                Text = newTextPart
                            });
                        }
                    }
                }

            }
            //ReAssignPartIndexOrder();
            //var lastPart = _parts.Last();
            //if (lastPart.Type == AutocompleteTextPartType.Suggestion)
            //{
            //    if (lastPart.Text.EndsWith(" ") && (lastPart as AutoCompleteArgumentSuggestPart).Suggest != null)
            //    {
            //        lastPart.Text = CommandPart.Text.TrimEnd(' ');
            //        _parts.Add(new AutocompleteTextPart()
            //        {
            //            Index = _parts.Count,
            //            Text = " "
            //        });
            //    }
            //}
        }

        private void ClearParts()
        {
            if (_parts.Count > 1)
            {
                //skip command delete
                for (int i = 1; i < _parts.Count; i++)
                {
                    
                    var part = _parts[i];

                    if (part.MarkAsDeleted)
                    {
                        _parts.RemoveAt(i);
                        i--;
                    }

                    //if (string.IsNullOrEmpty(part.Text))
                    //{
                    //    if (i != 0 && i != _parts.Count - 1)
                    //    {
                    //        if (_parts[i - 1].Text.EndsWith(" ") && _parts[i + 1].Text.StartsWith(" "))
                    //        {
                    //            continue;
                    //        }

                    //        //two next are empty, right will be deleted
                    //        if (string.IsNullOrEmpty(_parts[i + 1].Text))
                    //        {
                    //            _parts.RemoveAt(i+1);
                    //            i--;
                    //        }
                    //        else
                    //        {
                    //            _parts.RemoveAt(i);
                    //            i--;
                    //        }
                            
                    //    }
                        
                       
                    //}
                }
            }
        }

        private int GetStartPartPos(int partPos)
        {
            return _parts.Where(x => x.Index < partPos).Sum(x => x.Text.Length);
        }

        public override string ToString()
        {
            return PartsToString(_parts);
        }

        public string PartsToString(List<AutocompleteTextPart> parts)
        {
            return string.Join("", parts.Select(x => x.Text).ToArray());
        }

        public void SetCommand(CallCommandInfo command)
        {
            CommandPart.Text = command.Command;
            CommandPart.Command = command;
            _currCaretPos = CommandPart.Text.Length;
            _lastText = CommandPart.Text;
        }

        public void Clear()
        {
            _parts.Clear();
            AddEmptyCommand();
            _lastText = "";
            _changedEventEnabled = false;
            _bindedControl.Text = "";
            _changedEventEnabled = true;
        }

        public void RefreshText()
        {
            _changedEventEnabled = false;
            Console.WriteLine("RefreshText");
            _bindedControl.Text = "";//this.ToString();
            
            int pos = 0;
            foreach (AutocompleteTextPart part in _parts)
            {
                SetBackColor(part.BackColor);
                _bindedControl.AppendText(part.Text);
                pos += part.Text.Length;
            }
            _bindedControl.SelectionStart = _currCaretPos;
            _changedEventEnabled = true;
            
        }

        

        

        public void NotifyAboutAvailableCommandSuggests(List<string> commands)
        {
            //Console.WriteLine("NotifyAboutAvailableCommandSuggests()");
            //if (commands.Count == 1)
            //{
            //    //save command
            //    Command.RealCommand = commands.First();
            //}
            //else if (commands.Count == 0 && !string.IsNullOrEmpty(Command.RealCommand) && Command.Text.EndsWith(" ")) //full command entered before space
            //{
            //    SetCommand(Command.RealCommand, Command.Text);
            //}
            //else
            //{
            //    Command.RealCommand = null;
            //}
        }

        private AutoCompleteCommandPart CommandPart { get { return _parts.First() as AutoCompleteCommandPart; } }

        public void Log()
        {
            foreach (var part in _parts)
            {
                string details = string.Empty;
                if (part.Type == AutocompleteTextPartType.Command)
                {
                    var command = (part as AutoCompleteCommandPart);
                    details += "[" + (command.Command!=null?command.Command.Command:null) + "]";
                }
                if (part.Type == AutocompleteTextPartType.Suggestion)
                {
                    var command = (part as AutoCompleteArgumentSuggestPart);
                    details += "[" + (command.Suggest != null ? command.Suggest.Value : null) + "]";
                }
                Console.WriteLine("[" + part.Text + "] - " + part.Type + " " + details + Environment.NewLine);
            }
        }

        private void CheckForAvailableArgumentSuggestions(AutocompleteTextPart focusedPart)
        {
            if (CommandPart.Command != null)
            {
                if (CommandPart.Command.CommandArgumentSuggestions != null && !IsCommandInFocus(focusedPart))
                {
                    //string args = PartsToString(_parts.Where(x => x.Type != AutocompleteTextPartType.Command && x.Type != AutocompleteTextPartType.Suggestion && x.Type != AutocompleteTextPartType.Delimeter).ToList()).TrimStart();
                    List<string> argsList = new List<string>();

                    StringBuilder partsSb = new StringBuilder();
                    for (int i = 0; i < _parts.Count; i++)
                    {
                        var part = _parts[i];
                        if (part.Type == AutocompleteTextPartType.Command || part.Type == AutocompleteTextPartType.Delimeter) continue;
                        
                        if (part.Type == AutocompleteTextPartType.Suggestion )
                        {
                            if (partsSb.Length > 0)
                            {
                                argsList.Add(partsSb.ToString());
                                partsSb = new StringBuilder();
                            }
                            continue;
                        }
                        partsSb.Append(part.Text);
                        if (i == _parts.Count - 1)
                        {
                            if (partsSb.Length > 0)
                            {
                                argsList.Add(partsSb.ToString());
                            }
                        }
                    }


                    //todo : thread pool required
                    //new Thread(() =>
                    //{
                        
                    //}).Start();
                    try
                    {

                        for (int i = 0; i < CommandPart.Command.CommandArgumentSuggestions.Count; i++)
                        {
                            var comSuggestion = CommandPart.Command.CommandArgumentSuggestions[i];
                            if (comSuggestion.TemplateParseInfo.Count >= argsList.Count)
                            {
                                for (int j = 0; j < comSuggestion.TemplateParseInfo.Count; j++)
                                {
                                    
                                    var argSuggestion = comSuggestion.TemplateParseInfo[j];
                                    bool strongRestrictSuggest = string.IsNullOrEmpty(argSuggestion.RegexpPart);
                                    bool strongRestrictedPart = focusedPart.Type == AutocompleteTextPartType.Suggestion && (focusedPart as AutoCompleteArgumentSuggestPart).StrongRestrictSuggest;
                                    bool canBeSuggestedForRestrictSuggest = (strongRestrictSuggest && (string.IsNullOrEmpty(focusedPart.Text) || strongRestrictedPart));
                                    bool canBeSuggestedForRegextDefinedSuggest = (!strongRestrictSuggest && argsList.Any() && Regex.IsMatch(argsList[j], argSuggestion.RegexpPart));
                                    if (canBeSuggestedForRestrictSuggest || canBeSuggestedForRegextDefinedSuggest)
                                    {
                                        List<AutoSuggestItem> suggestions = comSuggestion.Details.Where(x => x.Key == argSuggestion.Key).Select(x => x.GetSuggestionFunc).Single()();


                                        if (focusedPart.Index == _parts.Last().Index && focusedPart.Type != AutocompleteTextPartType.Suggestion)//(!string.IsNullOrEmpty(focusedPart.Text) || canBeSuggestedForRestrictSuggest)
                                        {
                                            focusedPart = new AutoCompleteArgumentSuggestPart()
                                            {
                                                Index = _parts.Count,
                                                Text = "",
                                                StrongRestrictSuggest = strongRestrictSuggest
                                            };
                                            AppendParts(focusedPart);
                                        }

                                        var suggPart = focusedPart as AutoCompleteArgumentSuggestPart;
                                        if (suggPart != null)
                                        {
                                            var argToCompare = suggPart.Text.ToLower();
                                            
                                            if (!string.IsNullOrEmpty(suggPart.Text) && suggPart.StrongRestrictSuggest)
                                            {
                                                var canBeFound = suggestions.Any(x => x.Value.ToLower().StartsWith(argToCompare));
                                                if (!canBeFound)
                                                {
                                                    ChangePartType(focusedPart.Index,new AutocompleteTextPart()
                                                    {
                                                        Text=suggPart.Text
                                                    });
                                                }
                                            }
                                            
                                           var found = suggestions.FirstOrDefault(x => x.Value.ToLower() == argToCompare);
                                           suggPart.Suggest = found;
                                           suggPart.SuggestCases = suggestions;
                                            
                                            
                                        }
                                        return;
                                    }
                                    else
                                    {
                                        break; //do not scan next
                                    }
                                }


                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //if (OnModuleErrorOccured != null)
                        //    OnModuleErrorOccured(ex, command);
                    }
                }
            }
        }

        private void ChangePartType(int index, AutocompleteTextPart newPart)
        {
            newPart.Index = index;
            _parts[index] = newPart;
        }

        private void AppendParts(AutocompleteTextPart part,bool toEnd = true)
        {
            if (toEnd)
            {
                _parts.Add(part);
                DeletePartIfEmpty(_parts.Count - 2);
            }
            else
            {
                var focusedPart = GetFocusedPart();
                if (focusedPart == null)
                    AppendParts(part); //to the end
                else
                {
                    _parts.Insert(focusedPart.Index, part);
                    DeletePartIfEmpty(focusedPart.Index);
                }
            }
            ReAssignPartIndexOrder();
        }

        private void DeletePartIfEmpty(int partIndex)
        {
            if (_parts.Count > partIndex && partIndex>=0)
            {
                if (string.IsNullOrEmpty(_parts[partIndex].Text))
                {
                    _parts.RemoveAt(partIndex);
                }
            }
        }

        private void ReAssignPartIndexOrder()
        {
            for (int i = 0; i < _parts.Count; i++)
            {
                _parts[i].Index = i;
            }
        }
    }

    public class AutocompleteTextPart
    {
        public string Text { get; set; }
        public virtual AutocompleteTextPartType Type { get {return AutocompleteTextPartType.PartOfText;}  }
        public int Index { get; set; }
        public virtual Color BackColor {get {return Color.Transparent;}}
        public bool MarkAsDeleted { get; set; }

        public AutocompleteTextPart()
        {
            Text = "";
        }

        public virtual void Delete()
        {
            Text = "";
            MarkAsDeleted = true;
        }
    }
    
    public class AutoCompleteCommandPart : AutocompleteTextPart
    {
        public override AutocompleteTextPartType Type{get { return AutocompleteTextPartType.Command; }}
        public CallCommandInfo Command { get; set; }

        public override Color BackColor
        {
            get { return Color.Transparent; }
        }

        public override void Delete()
        {
            Command = null;
            base.Delete();
        }
    }
    public class AutoCompleteDelimeterPart : AutocompleteTextPart
    {
        public override AutocompleteTextPartType Type { get { return AutocompleteTextPartType.Delimeter; } }
    }
    public class AutoCompleteArgumentSuggestPart : AutocompleteTextPart
    {
        public override AutocompleteTextPartType Type { get { return AutocompleteTextPartType.Suggestion; } }
        public AutoSuggestItem Suggest { get; set; }
        public List<AutoSuggestItem> SuggestCases { get; set; }
        public bool StrongRestrictSuggest { get; set; }

        
        public override Color BackColor
        {
            get { return CreateRandomColor(); }
        }
        private Color CreateRandomColor()
        {

            Color randomColor = Color.FromArgb(158, 213, 136);
            Console.Write(randomColor);
            return randomColor;
        }

        public AutoCompleteArgumentSuggestPart()
        {
            SuggestCases = new List<AutoSuggestItem>();
        }
    }

    public enum AutocompleteTextPartType
    {
        Suggestion,
        Command,
        PartOfText,
        Delimeter
    }
}
