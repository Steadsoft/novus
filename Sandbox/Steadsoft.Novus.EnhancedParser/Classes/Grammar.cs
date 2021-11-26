using Steadsoft.Novus.Scanner.Classes;
using System.Collections.Generic;

namespace Steadsoft.Novus.EnhancedParser.Classes
{
    // see: https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/namespaces

    // non-terminals that are composed of several alternative sub non-termoianls
    // are reprsented with an empty interface.

    // compilation_unit is an exception because it can never appears on the RHS of any production.


    public class compilation_unit : namespace_member_declaration
    {
        public namespace_body namespace_body { get; internal init;} = new namespace_body();
        public global_attributes global_attributes { get; internal set; } = default;
    }

    public interface using_directive { }

    public interface namespace_or_type_name { }

    public class qualified_typename
    {
        public List<identifier_type_argument_list> fullname { get; internal init; } = new();
    }

    public class identifier_type_argument_list : namespace_or_type_name
    {
        public string identifier { get; internal set; } = null;
        public type_argument_list type_argument_list { get; internal init; } = new();
    }

    public class qualified_alias_member : namespace_or_type_name
    {
        public string first_part { get; internal set; } = null;
        public identifier_type_argument_list identifier_type_argument_list { get; internal set; } = new();
    }

    public class type_argument_list
    {
        List<identifier_type_argument_list> args;
    }

    public class using_alias_directive : using_directive
    {
        public string identifier { get; internal set; } = null;

    }

    public class using_namespace_directive : using_directive
    {

    }

    public class using_static_directive : using_directive
    {

    }

    public interface extern_alias_directive { }
    public interface global_attributes { }
    public interface namespace_member_declaration { }

    public class namespace_declaration : namespace_member_declaration
    {
        public Token Token { get; internal init; }
        public namespace_body namespace_body { get; internal init; } = new();
        public qualified_identifier name { get; internal init; } = new();

        public namespace_declaration(Token Token)
        {
            this.Token = Token; // used to record the line etc at which the code element began.
        }

        void AddMember(namespace_member_declaration Member)
        {
            namespace_body.namespace_member_declarations.Add(Member);
        }
    }
    public class namespace_body
    {
        public List<extern_alias_directive> extern_alias_directives { get; internal init; } = new();
        public List<using_directive> using_directives { get; internal init; } = new();
        public List<namespace_member_declaration> namespace_member_declarations { get; internal init; } = new();

        public void Add(extern_alias_directive extern_alias_directive)
        {
            extern_alias_directives.Add(extern_alias_directive);
        }
        public void Add(using_directive using_directive)
        {
            using_directives.Add(using_directive);
        }
        public void Add(namespace_member_declaration namespace_member_declaration)
        {
            namespace_member_declarations.Add(namespace_member_declaration);
        }

        public namespace_body()
        {

        }
    }

    public interface type_declaration : namespace_member_declaration { }

    public class class_declaration : type_declaration
    {

    }

    public class struct_declaration : type_declaration
    {

    }

    public class interface_declaration : type_declaration
    {

    }

    public class enum_declaration : type_declaration
    {

    }

    public class delegate_declaration : type_declaration
    {

    }

    public class qualified_identifier
    {

    }


    //public class using_alias_directive : using_directive 
    //{

    //}

    //public class using_namespace_directive : using_directive
    //{

    //}

    //public class using_static_directive : using_directive
    //{

    //}

    public class GlobalAttribute
    {

    }

    

    public interface namespace_member
    {

    }

    public class @namespace : namespace_member
    {
        NamespaceName namespace_name;
        namespace_member parent;
        List<namespace_member> namespace_members = new();
        void AddMember(namespace_member Member)
        {
            namespace_members.Add(Member);
        }
    }

    public class type : namespace_member
    {
        List<general_type_member> type_members = new();

        public type()
        {
            type_members.Add(new Field());
            //type_members.Add(new Accessibility());
        }
    }

    public interface basic_type_member
    { 
    
    }
    public interface general_type_member 
    {

    }
    public class Constant : basic_type_member, general_type_member
    {

    }

    public class Field : basic_type_member, general_type_member
    {

    }

    public class Method : basic_type_member, general_type_member
    {

    }

    public class Property : basic_type_member, general_type_member
    {

    }

    //public class Accessibility : general_type_member
    //{
    //    List<basic_type_member> type_members = new();

    //    public Accessibility()
    //    {
    //        type_members.Add(new Field());
    //        type_members.Add(new Accessibility());
    //    }
    //}

    public class NamespaceName
    {
        public string name;
    }

    public class TypeName
    {
        public string name;
        public TypeParameters type_paramaters;
    }

    public class TypeParameters
    {

    }
}