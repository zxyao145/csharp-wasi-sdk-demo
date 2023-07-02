#include <mono-wasi/driver.h>
#include <assert.h>
#include <stdio.h>

__attribute__((__import_module__("env"), __import_name__("hello_from_env")))
extern void __wasm_import_env_hello();


__attribute__((__import_module__("go-wasm-module"), __import_name__("add")))
extern int __wasm_import_go_add(int x, int y);

void attach_internal_calls() {
    mono_add_internal_call("WasmLib.WasmImport::HelloFromEnv", __wasm_import_env_hello);
    mono_add_internal_call("WasmLib.WasmImport::GoWasmAdd", __wasm_import_go_add);
}
