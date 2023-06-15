#include <mono-wasi/driver.h>
#include <assert.h>
#include <stdio.h>

MonoDomain* mono_get_root_domain(void);


MonoMethod* method_HandleIncomingRequest;
__attribute__((export_name("run")))
void run() {
    if (!method_HandleIncomingRequest) {
        method_HandleIncomingRequest =
            lookup_dotnet_method("WasmLib.dll", "WasmLib", "WasmClass", "Run", -1);
    }

    MonoObject* exception;
    void* method_params[] = { };
    mono_wasm_invoke_method(method_HandleIncomingRequest, NULL, method_params, &exception);

    assert(!exception);
}



// string paramter start

MonoMethod* method_HandleEcho;
__attribute__((export_name("echo")))
void echo(int times, char* msg) {
    if (!method_HandleEcho) {
        method_HandleEcho =
            lookup_dotnet_method("WasmLib.dll", "WasmLib", "WasmClass", "Echo", -1);
    }
    MonoObject* exception;
    MonoString* msg_trans = mono_wasm_string_from_js(msg);
    void* method_params[] = {
        &times,
        msg_trans
    };
    mono_wasm_invoke_method(
        method_HandleEcho,
        NULL,
        method_params,
        &exception
    );
    assert(!exception);
    free(msg);
}

// string paramter end



// byte[] paramter start
MonoClass* mono_get_byte_class(void);
int mono_unbox_int(MonoObject* obj);

// 用于将调用方的指针和数组长度生成为c#中的数组
MonoArray* mono_wasm_typed_array_new(void* arr, int length) {
    MonoClass* typeClass = mono_get_byte_class();
    MonoArray* buffer = mono_array_new(mono_get_root_domain(), typeClass, length);
    // 1 == sizeof(byte)
    int p = mono_array_addr_with_size(buffer, 1, 0);
    // length == length * sizeof(byte)
    memcpy(p, arr, length);
    return buffer;
}

// 存根函数
MonoMethod* method_HandleByteArrayParam;
__attribute__((export_name("byte_array_param")))
int byte_array_param(void* nrs_ptr, int nrs_len)
{
    if (!method_HandleByteArrayParam) {
        method_HandleByteArrayParam =
            lookup_dotnet_method("WasmLib.dll", "WasmLib", "WasmClass", "ByteArrayParam", -1);
    }

    MonoObject* exception;
    MonoArray* nrs_trans = nrs_ptr ? mono_wasm_typed_array_new(nrs_ptr, nrs_len) : NULL;

    void* method_params[] = {
        nrs_trans
    };
    MonoObject* res = mono_wasm_invoke_method(
        method_HandleByteArrayParam,
        NULL,
        method_params,
        &exception
    );

    assert(!exception);
    free(nrs_ptr);

    // C#返回的是一个指针，MonoObject 是对其的装箱，需要使用 mono_unbox_int 将其拆箱为一个 int 值（指针地址）
    int p = mono_unbox_int(res);
    return p;
}

// byte[] paramter end



// int[] paramter start
MonoClass* mono_get_int32_class(void);
int mono_unbox_int(MonoObject* obj);

// 用于将调用方的指针和数组长度生成为c#中的数组
MonoArray* mono_wasm_int32_typed_array_new(void* arr, int length) {
    MonoClass* typeClass = mono_get_int32_class();
    MonoArray* buffer = mono_array_new(mono_get_root_domain(), typeClass, length);
    int p = mono_array_addr_with_size(buffer, sizeof(int), 0);
    memcpy(p, arr, length * sizeof(int));
    return buffer;
}

// 存根函数
MonoMethod* method_HandleIntArrayParam;
__attribute__((export_name("int_array_param")))
void int_array_param(void* nrs_ptr, int nrs_len)
{
    if (!method_HandleIntArrayParam) {
        method_HandleIntArrayParam =
            lookup_dotnet_method("WasmLib.dll", "WasmLib", "WasmClass", "IntArrayParam", -1);
    }

    MonoObject* exception;
    MonoArray* nrs_trans = nrs_ptr ? mono_wasm_int32_typed_array_new(nrs_ptr, nrs_len) : NULL;

    void* method_params[] = {
        nrs_trans
    };
    
    mono_wasm_invoke_method(
        method_HandleIntArrayParam,
        NULL,
        method_params,
        &exception
    );

    assert(!exception);
    free(nrs_ptr);
}

// int[] paramter end
