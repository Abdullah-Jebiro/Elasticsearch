﻿// See https://aka.ms/new-console-template for more information
using System.ComponentModel.DataAnnotations;

public class UpdateEntitie
{
    [Key]
    public int EntityId { get; set; }
    public DateTime DateTime { get; set; }
    public string TypeEntities { get; set; } 

}
