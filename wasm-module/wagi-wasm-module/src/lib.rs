use std::io::{stdin, self, Write};


#[no_mangle]
pub fn main() {
    // Request can be read from standard input
    // The response will be read from the standard output

    // output header
    // Two newlines are used to split the header and body of the response
    println!("Content-Type: text/plain\n\n");

    // The requested path (X_MATCHED_ROUTE), 
    // HTTP_User_Agent, X_FULL_URL, CONTENT_TYPE, SERVER_NAME, SERVER_PORT, etc. 
    // can be read from environment variables
    std::env::vars().for_each(|v| {
        println!("{} = {}", v.0, v.1);
    });

    //  The requested query paramter, can be read from args
    for arg in std::env::args()
    {
        // arg: a=1
        println!("arg: {}", arg);
    }

    // read body
    let iterator = stdin().lines();
    let mut lines_str = String::new();
    for line in iterator {
        let s = line.unwrap();
        println!("line: {}", s);
        lines_str.push_str(&s)
    }

    let stdout = io::stdout();
    let mut w = io::BufWriter::new(stdout);
    _ = writeln!(w, "hello: {}", lines_str); 
}
