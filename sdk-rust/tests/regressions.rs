use hime_sdk::errors::print::{get_line_number_width, print_error};
use hime_sdk::output::helper::{get_namespace_java, get_namespace_net, get_namespace_rust};
use hime_sdk::{CompilationTask, Input, ParsingMethod};

/// [Github issue #79](https://github.com/cenotelie/hime/issues/79)
#[test]
fn test_errors_display() {
    let mut task = CompilationTask::default();
    task.inputs.push(Input::Raw(
        "grammar Test {
            options {
                Axiom = \"axiom\";
            }
            terminals {
                A->'a';
            }
            rules {
                s -> A s | ;
                axiom -> s A ;
            }
        }"
    ));
    task.method = Some(ParsingMethod::LALR1);
    let mut data = task.load().unwrap();
    let errors = match task.generate_in_memory(&mut data.grammars[0], 0) {
        Ok(_) => panic!("expected errors"),
        Err(e) => e
    };
    for error in errors.iter() {
        // should work
        let width = get_line_number_width(error, &data);
        print_error(error, width, &data);
    }
}

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
