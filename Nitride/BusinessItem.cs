/// ***************************************************************************
/// Nitride Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// ***************************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;

namespace Nitride.Business
{
    public class BusinessItem : IOrdered, IObject
    {
        public string Name { get; set; }

        public string Label { get; set; }

        public string Description { get; set; }

        public bool Enabled { get; set; }

        public int Order { get; set; }

        public Importance Importance { get; set; }

        public Dictionary<string, string> Tags { get; } = new();
    }



    public abstract class Entity : BusinessItem
    {
        public Dictionary<string, string> Ids { get; } = new();

        public const string Shipping_t = "Shipping";

        public const string Billing_t = "Billing";

        public const string Default_t = "Default";

        public Dictionary<string, Address> Addresses { get; } = new();

        public Email Email { get; }


    }


    public class Email
    {
        public string UserName { get; }

        public string Domain { get; }
    }

    public class Company : Entity//, IEnumerable<Person>
    {
        public string EID => Ids.ContainsKey(EID_t) ? Ids[EID_t] : null;


        public const string EID_t = "EID";

        // TODO: Tree structure
        public List<Group> Groups { get; } = new();
    }



    public class Person : Entity, IEquatable<Person>
    {
        public string FirstName { get; }

        public string LastName { get; }

        public string Title { get; set; }

        public Gender Gender { get; set; }

        public const string SSN_t = "SSN";

        public bool Equals(Person other)
        {
            

            throw new NotImplementedException();
        }

        //public string SSN => Ids.ContainsKey(EID_t) ? Ids[EID_t] : null;
    }


    public class Employee : IEquatable<Person>
    {
        public string Name => Person.Name;

        public Person Person { get; set; }

        public string Id { get; set; }

        public Role Role { get; set; }

        public bool Equals(Person other) => Person is Person p && p.Equals(other);
    }

    public class Group : BusinessItem
    {

        public List<Employee> Members { get; } = new();

        public IEnumerable<Person> People => Members.Select(n => n.Person).Distinct();



    }

    public class Role 
    {

        public string Title { get; set; }

        public string JobDescription { get; set; }
    
    }


    public enum Gender : int
    {
        NotSure = -1,
        Male = 0,
        Female = 1,
        Other = 2,
        Flexible = 3
    }

    public abstract class Service 
    {
    
    
    }

    public interface IServiceable
    {
        IEnumerable<Service> Services { get; }
    

        Dictionary<Entity, Service> Service { get; }
    }




    public class Part : BusinessItem
    {




    }

    public class SourcePart 
    {
        public string PartNumber { get; set; }

        public Entity Vendor { get; set; }

        public Dictionary<(Entity Entity, double Quantity), Quote> Quotes { get; set; }
    }

    public class Quote
    {
        public double Quantity { get; set; }

        public double Price { get; set; }

        public DateTime QuoteTime { get; set; }

        public DateTime Expiration { get; set; }
    }

    public class PcbTool 
    {
        public string Name { get; }
    }

    public class PcbPart : Part
    {
        public Dictionary<PcbTool, PcbPartConfiguration> PcbPartConfigurations { get; } = new();

        public bool IsSMT { get; set; }

        public string FootprintSizeX { get; }

        public string FootprintSizeY { get; }

        public string FootprintHeight { get; }

        public string Certification { get; }

        public double TemperatureLow { get; set; }

        public double TemperatureHigh { get; set; }

        public List<SourcePart> SourceParts { get; } = new();
    }

    public class PcbPartConfiguration : IObject
    {
        public string Name { get; set; }

        public string Label { get; set; }

        public string Description { get; set; }

        public string FootprintOfficiaName { get; set; }

        public string Certification { get; }

        public string SymbolName { get; }

        public string SymbolContainer { get; }

        public string FootprintName { get; }

        public string FootprintContainer { get; }
    }


    //public class 
}