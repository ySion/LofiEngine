using System.Dynamic;
using System.Text;
using Vortice.Vulkan;
using System.Runtime.InteropServices;

namespace LofiEngine.Gfx;

public class Context {
      public Context() {
            Vulkan.vkInitialize();
      }

      public unsafe void InitInstance() {
            using ToPointerString applicationName = new ToPointerString("hello world");
            using ToPointerString engineName = new ToPointerString("Engine Name");

            VkApplicationInfo appInfo = new VkApplicationInfo {
                  pApplicationName = applicationName.pointer,
                  applicationVersion = new VkVersion(1, 0, 0),
                  pEngineName = engineName.pointer,
                  engineVersion = new VkVersion(1, 0, 0),
                  apiVersion = VkVersion.Version_1_3
            };

            using ToPointerStringList layers = new(["VK_LAYER_KHRONOS_validation"]);
            using ToPointerStringList extensions = new(["VK_KHR_surface", "VK_KHR_win32_surface"]);

            VkInstanceCreateInfo instanceCi = new VkInstanceCreateInfo {
                  pApplicationInfo = &appInfo,
                  enabledLayerCount = layers.Count,
                  ppEnabledLayerNames = layers.Pointer,
                  enabledExtensionCount = extensions.Count,
                  ppEnabledExtensionNames = extensions.Pointer,
                  pNext = null
            };

            VkResult res = Vulkan.vkCreateInstance(instanceCi, null, out VkInstance instance);
            if (res != VkResult.Success) {
                  Console.WriteLine($"Create Error {res}");
            } else {
                  Console.WriteLine("Create Success");
            }
      }


      public void InitDevice() {
      }

      public VkDevice device;

      public VkInstance instance;
}