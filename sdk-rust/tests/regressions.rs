use hime_sdk::output::helper::{get_namespace_java, get_namespace_net, get_namespace_rust};

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
