using Wasmtime;

namespace WasmDemo
{
    internal class WasmHelper:IDisposable
    {
        public Engine Engine { get; private set; }
        public Store Store { get; private set; }
        public Linker Linker { get; private set; }
        public Module? Module { get; private set; } = null;

        private Instance? _instance = null;
        public Instance Instance
        {
            get
            {
                if (_instance == null && Module != null)
                {
                    Init();
                }

                return _instance;
            }
        }

        public WasmHelper()
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
        }

        public WasmHelper(string wasmFile):this()
        {
            Module = Module.FromFile(Engine, wasmFile);
        }

        

        private void Init()
        {
            _instance = GetInstance(Module!);
        }

        public Instance GetInstance(Module module)
        {
            return Linker.Instantiate(Store, module);
        }

        public void Dispose()
        {
            Engine?.Dispose();
            Store?.Dispose();
            Linker?.Dispose();
            Module?.Dispose();
        }
    }
}
