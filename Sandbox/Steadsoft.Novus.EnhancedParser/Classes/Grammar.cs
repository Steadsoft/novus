using Steadsoft.Novus.Scanner.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Steadsoft.Novus.EnhancedParser.Classes
{

    // see: https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/namespaces

    // non-terminals that are composed of several alternative sub non-termoianls
    // are reprsented with an empty interface.

    // compilation_unit is an exception because it can never appears on the RHS of any production.


    public class compilation_unit
    {
        public namespace_body namespace_body = new namespace_body();
        public global_attributes global_attributes = default;
    }

    public interface using_directive { }

    public interface namespace_or_type_name { }

    public class identifier_type_argument_list : namespace_or_type_name
    {
        string identifier;
        type_argument_list type_argument_list = new();
    }

    public class qualified_alias_member : namespace_or_type_name
    {
        string first_part;
        string second_part;
        type_argument_list type_argument_list = new();
    }

    public class type_argument_list
    {

    }

    public class using_alias_directive : using_directive
    {
        string identifier;

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
        public Token Token { get; private set; }
        namespace_body namespace_body = new();
        qualified_identifier name = new();

        public namespace_declaration(Token Token)
        {
            this.Token = Token; // used to record the line etc at which the code element began.
            namespace_body.namespace_member_declarations.Add(new class_declaration()); // TODO remove, compile test only.
        }

        void AddMember(namespace_member_declaration Member)
        {
            namespace_body.namespace_member_declarations.Add(Member);
        }
    }
    public class namespace_body
    {
        public List<extern_alias_directive> extern_alias_directives = new();
        public List<using_directive> using_directives = new();
        public List<namespace_member_declaration> namespace_member_declarations = new();

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