using Wasmtime;

namespace WasmDemo
{
    internal class WasmHelper:IDisposable
    {
        public Engine Engine { get; private set; }
        public Store Store { get; private set; }
        public Linker Linker { get; private set; }
        public Module Module { get; private set; }
        public Instance Instance { get; private set; }


        public WasmHelper(string wasmFile)
        {
            var wasi = new WasiConfiguration()
                .WithInheritedStandardInput()
                .WithInheritedStandardOutput()
                .WithInheritedStandardError();

            Engine = new Engine();
            Store = new Store(Engine);
            Store.SetWasiConfiguration(wasi);

            Linker = new Linker(Engine);
            Linker.DefineWasi();

            Module = Module.FromFile(Engine, wasmFile);

            Instance = Linker.Instantiate(Store, Module);
        }

        public void Dispose()
        {
            Engine.Dispose();
            Store.Dispose();
            Linker.Dispose();
            Module.Dispose();
        }
    }
}
