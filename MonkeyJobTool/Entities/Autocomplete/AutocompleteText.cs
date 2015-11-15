﻿using System;
using System.Collections.Generic;
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
        private readonly TextBox _bindedControl;
        

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

        public AutocompleteText(TextBox bindedControl, TryResolveCommandFromStringDelegate tryResolveCommandFromStringFunc)
        {
            _bindedControl = bindedControl;
            _tryResolveCommandFromStringFunc = tryResolveCommandFromStringFunc;

            _bindedControl.MouseDown += (sender, args) => { changeCaretPos(); };
            _bindedControl.MouseUp += (sender, args) => { changeCaretPos(); };
            _bindedControl.TextChanged += (sender, args) =>
            {
                changeCaretPos(true);
                if (_changedEventEnabled)
                {
                    textChanged();
                    ClearParts();
                    CorrectParts();
                    TryResolveCommandIfRequired();
                    TryNotifyAboutCommandFocused();
                    TryNotifyAboutCommandBlured();

                    CheckForAvailableArgumentSuggestions();
                    
                    Log();
                }
                
            };
            _bindedControl.KeyUp += (sender, args) => { changeCaretPos(); };
            AddEmptyCommand();
            _lastText = bindedControl.Text;
        }

        private void AddEmptyCommand()
        {
            _parts = new List<AutocompleteTextPart>()
            {
                new AutoCompleteCommandPart()
                {
                   Index = 0
                }
            };
        }

        private void changeCaretPos(bool fromChangedTextEvent = false)
        {
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
        private void TryResolveCommandIfRequired()
        {
            if (IsCommandInFocus())
            {
                Console.WriteLine("_tryResolveCommandFromStringFunc()");
                var command = _tryResolveCommandFromStringFunc(CommandPart.Text);
                CommandPart.Command = command;
            }
        }
        private void TryNotifyAboutCommandFocused()
        {
            if (IsCommandInFocus())
            {
                if (OnCommandFocused != null)
                    OnCommandFocused(CommandPart.Text);
            }
        }
        private void TryNotifyAboutCommandBlured()
        {
            if (!IsCommandInFocus())
            {
                if (OnCommandBlured != null)
                    OnCommandBlured();
            }
        }

        private bool IsCommandInFocus()
        {
            return _currCaretPos <= CommandPart.Text.Length && !string.IsNullOrEmpty(CommandPart.Text);
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

                    break;
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
                        changedPart.Text = "";
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
            if (CommandPart.Text.EndsWith(" ") && CommandPart.Command!=null)
            {
                CommandPart.Text = CommandPart.Text.TrimEnd(' ');
                _parts.Add(new AutocompleteTextPart()
                {
                    Index = _parts.Count,
                    Text = " "
                });
            }

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
                for (int i = 0; i < _parts.Count; i++)
                {
                    
                    var part = _parts[i];
                    part.Index = i;
                    //skip command part removing
                    if(part.Type==AutocompleteTextPartType.Command) 
                        continue;
                    
                    if (string.IsNullOrEmpty(part.Text))
                    {
                        if (i != 0 && i != _parts.Count - 1)
                        {
                            if (_parts[i - 1].Text.EndsWith(" ") && _parts[i + 1].Text.StartsWith(" "))
                            {
                                continue;
                            }
                        }
                        _parts.RemoveAt(i);
                        i--;
                    }
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
        }

        public void Clear()
        {
            _parts.Clear();
            AddEmptyCommand();
        }

        public void RefreshText()
        {
            _changedEventEnabled = false;
            _bindedControl.Text = this.ToString();
            _bindedControl.SelectionStart = _bindedControl.Text.Length;
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

        private void CheckForAvailableArgumentSuggestions()
        {
            if (CommandPart.Command != null)
            {
                if (CommandPart.Command.CommandArgumentSuggestions != null && !IsCommandInFocus())
                {
                    string args = PartsToString(_parts.Where(x => x.Type != AutocompleteTextPartType.Command && x.Type != AutocompleteTextPartType.Suggestion).ToList()).TrimStart();
                    //todo : thread pool required
                    //new Thread(() =>
                    //{
                        
                    //}).Start();
                    try
                    {
                        for (int i = 0; i < CommandPart.Command.CommandArgumentSuggestions.Count; i++)
                        {
                            var comSuggestion = CommandPart.Command.CommandArgumentSuggestions[i];
                            foreach (var argSuggestion in comSuggestion.TemplateParseInfo)//.Where(x => x.Order == _parts.Count(y => y.Type == AutocompleteTextPartType.Suggestion))
                            {
                                if (args.Trim() != "")
                                {
                                    if (!string.IsNullOrEmpty(argSuggestion.RegexpPart) && Regex.IsMatch(args, argSuggestion.RegexpPart))
                                    {
                                        List<AutoSuggestItem> suggestions = comSuggestion.Details.Where(x => x.Key == argSuggestion.Key).Select(x => x.GetSuggestionFunc).Single()();
                                        //todo : check case when text pasted in the middle ?
                                        if (_parts.Last().Type != AutocompleteTextPartType.Suggestion)
                                        {
                                            _parts.Add(new AutoCompleteArgumentSuggestPart()
                                            {
                                                Index = _parts.Count,
                                                Text = ""
                                            });
                                        }

                                        if (_parts.Last().Type == AutocompleteTextPartType.Suggestion)
                                        {
                                            var suggPart = _parts.Last() as AutoCompleteArgumentSuggestPart;
                                            var argToCompare = suggPart.Text.ToLower();
                                            var found = suggestions.FirstOrDefault(x => x.Value.ToLower() == argToCompare);

                                            
                                            if (found == null && suggPart.Suggest != null &&
                                                suggPart.Suggest.Value.Length <= suggPart.Text.Length //in case of delete 
                                                )
                                            {
                                                var newTextPart = suggPart.Text.Substring(suggPart.Suggest.Value.Length);
                                                suggPart.Text = suggPart.Text.Substring(0, suggPart.Suggest.Value.Length);
                                                _parts.Add(new AutocompleteTextPart()
                                                {
                                                    Index = _parts.Count,
                                                    Text = newTextPart
                                                });
                                            }

                                            suggPart.Suggest = found;
                                        }
                                        return;
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
    }

    public class AutocompleteTextPart
    {
        public string Text { get; set; }
        public virtual AutocompleteTextPartType Type { get {return AutocompleteTextPartType.PartOfText;}  }
        public int Index { get; set; }
        
        public AutocompleteTextPart()
        {
            Text = "";
        }
    }

    public class AutoCompleteCommandPart : AutocompleteTextPart
    {
        public override AutocompleteTextPartType Type{get { return AutocompleteTextPartType.Command; }}
        public CallCommandInfo Command { get; set; }
    }

    public class AutoCompleteArgumentSuggestPart : AutocompleteTextPart
    {
        public override AutocompleteTextPartType Type { get { return AutocompleteTextPartType.Suggestion; } }
        public AutoSuggestItem Suggest { get; set; }
        

    }

    public enum AutocompleteTextPartType
    {
        Suggestion,
        Command,
        PartOfText,
    }
}
