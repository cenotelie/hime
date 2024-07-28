use std::borrow::BorrowMut;

use hime_sdk::{output::helper::{get_namespace_java, get_namespace_net, get_namespace_rust}, ParsingMethod};

/// [Github issue #79](https://github.com/cenotelie/hime/issues/79)
#[test]
fn test_namespace_transformation() {
    assert_eq!(get_namespace_java("a.b.c"), String::from("a.b.c"));
    assert_eq!(get_namespace_java("a::b::c"), String::from("a.b.c"));
    assert_eq!(get_namespace_net("a.b.c"), String::from("A.B.C"));
    assert_eq!(get_namespace_net("a::b::c"), String::from("A.B.C"));
    assert_eq!(get_namespace_rust("a.b.c"), String::from("a::b::c"));
    assert_eq!(get_namespace_rust("a::b::c"), String::from("a::b::c"));
}

/// [Github issue #109](https://github.com/cenotelie/hime/issues/109)
#[test]
pub fn test_in_memory_parser(){
    let text_grammar = r#"
    grammar Test
    {
        options
        {
            Axiom = "expression";
        }
        terminals
        {
            A -> 'a'+;
        }
        rules
        {
            expression -> A ;
        }
    }
    "#;

    let input = hime_sdk::Input::Raw(&text_grammar);
    let inputs = vec![input];
    if let Ok(l) = hime_sdk::loaders::load_inputs(&inputs) {
        // The load_inputs allows to load multiple inputs and turn them into grammars,
        // but in our case, we only have one input grammar
        let mut grammars = l.grammars;            
        let input_grammar = grammars[0].borrow_mut();
        if let Ok(data) = input_grammar.build(Some(ParsingMethod::LR1), 0) {
            if let Ok(parser) = hime_sdk::output::build_in_memory_grammar(input_grammar, &data) {
                let res_1 = parser.parse("aaa");
                assert!(res_1.is_success()); 
                let res_2 = parser.parse("aaab");
                assert!(!res_2.is_success()); 
            }
        }   
    }
    
}