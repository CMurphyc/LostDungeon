﻿using UnityEngine;
using System.Collections;



public class Buff
{
    public bool Undefeadted = false;
    public int Undefeadted_RemainingFrame;


    public bool AttackIncrease = false;
    public int AttackIncrease_RemainingFrame;

    public bool Invisible = false;
    public int Invisible_RemainingFrame;
}


public class DeBuff
{
    public Debuff_Freeze Freeze = new Debuff_Freeze();

    /*
    public bool dd{get;set;}
    public bool dd{get;set;}
    public bool dd{get;set;}
    public bool dd{get;set;}
    public bool dd{get;set;}
    public bool dd{get;set;}
    public bool dd{get;set;}
    public bool dd{get;set;}
    public bool dd{get;set;}
    public bool dd{get;set;}
    */
}




public class Debuff_Freeze
{
    public bool isFreeze = false;
    public int RemainingFrame;
}