using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BotDataBetweenDialogs.Models
{
    //It is importnat that models are serializable so that they can be serialsied to bot state.
    [Serializable]
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}