﻿using System.Windows.Controls;

namespace CHDesktop.PersonalElements;

public partial class MessageControl : UserControl
{
    public string? TextDangerMessage { get; set; }
    public string? TextMessage { get; set; }
    public MessageControl()
    {
        InitializeComponent();
        DataContext = this;
    }
}
