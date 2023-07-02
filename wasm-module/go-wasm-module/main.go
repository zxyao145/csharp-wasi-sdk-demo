package main

//export add
func add(x, y uint32) uint32 {
	return x + y
}

// main is required for the `wasi` target, even if it isn't used.
func main() {
	// a := []byte("李qwertc")
	// b := []byte("张三")
	// var buf bytes.Buffer
	// buf.Write(a)
	// buf.Write(b)
	// // c := []byte("qwert张三");
	// fmt.Println(buf.String())

	// ct := time.Now()
	// fmt.Println(ct.UnixNano())
	// ctNano := time.Duration(ct.UnixNano())
	// ctSecInNano := time.Duration(ct.Unix()) * time.Nanosecond
	// max := (ctNano - ctSecInNano) / time.Microsecond

	// fmt.Println(max)

	// r3 := uint32(ct.Unix() / 60)
	// fmt.Println(r3)
}
