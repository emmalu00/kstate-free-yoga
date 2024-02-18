// vite.config.js
import { fileURLToPath, URL } from "node:url";
import { defineConfig } from "file:///C:/Users/Emma%20Lucas/OneDrive%20-%20Kansas%20State%20University/Senior%20Project/KSUFreeYoga/KStateFreeYoga/kstatefreeyoga.client/node_modules/vite/dist/node/index.js";
import plugin from "file:///C:/Users/Emma%20Lucas/OneDrive%20-%20Kansas%20State%20University/Senior%20Project/KSUFreeYoga/KStateFreeYoga/kstatefreeyoga.client/node_modules/@vitejs/plugin-vue/dist/index.mjs";
import fs from "fs";
import path from "path";
import child_process from "child_process";
var __vite_injected_original_import_meta_url = "file:///C:/Users/Emma%20Lucas/OneDrive%20-%20Kansas%20State%20University/Senior%20Project/KSUFreeYoga/KStateFreeYoga/kstatefreeyoga.client/vite.config.js";
var baseFolder = process.env.APPDATA !== void 0 && process.env.APPDATA !== "" ? `${process.env.APPDATA}/ASP.NET/https` : `${process.env.HOME}/.aspnet/https`;
var certificateArg = process.argv.map((arg) => arg.match(/--name=(?<value>.+)/i)).filter(Boolean)[0];
var certificateName = certificateArg ? certificateArg.groups.value : "kstatefreeyoga.client";
if (!certificateName) {
  console.error("Invalid certificate name. Run this script in the context of an npm/yarn script or pass --name=<<app>> explicitly.");
  process.exit(-1);
}
var certFilePath = path.join(baseFolder, `${certificateName}.pem`);
var keyFilePath = path.join(baseFolder, `${certificateName}.key`);
if (!fs.existsSync(certFilePath) || !fs.existsSync(keyFilePath)) {
  if (0 !== child_process.spawnSync("dotnet", [
    "dev-certs",
    "https",
    "--export-path",
    certFilePath,
    "--format",
    "Pem",
    "--no-password"
  ], { stdio: "inherit" }).status) {
    throw new Error("Could not create certificate.");
  }
}
var vite_config_default = defineConfig({
  plugins: [plugin()],
  resolve: {
    alias: {
      "@": fileURLToPath(new URL("./src", __vite_injected_original_import_meta_url))
    }
  },
  server: {
    proxy: {
      "^/weatherforecast": {
        target: "https://localhost:5001/",
        secure: false
      }
    },
    port: 5173,
    https: {
      key: fs.readFileSync(keyFilePath),
      cert: fs.readFileSync(certFilePath)
    }
  }
});
export {
  vite_config_default as default
};
//# sourceMappingURL=data:application/json;base64,ewogICJ2ZXJzaW9uIjogMywKICAic291cmNlcyI6IFsidml0ZS5jb25maWcuanMiXSwKICAic291cmNlc0NvbnRlbnQiOiBbImNvbnN0IF9fdml0ZV9pbmplY3RlZF9vcmlnaW5hbF9kaXJuYW1lID0gXCJDOlxcXFxVc2Vyc1xcXFxFbW1hIEx1Y2FzXFxcXE9uZURyaXZlIC0gS2Fuc2FzIFN0YXRlIFVuaXZlcnNpdHlcXFxcU2VuaW9yIFByb2plY3RcXFxcS1NVRnJlZVlvZ2FcXFxcS1N0YXRlRnJlZVlvZ2FcXFxca3N0YXRlZnJlZXlvZ2EuY2xpZW50XCI7Y29uc3QgX192aXRlX2luamVjdGVkX29yaWdpbmFsX2ZpbGVuYW1lID0gXCJDOlxcXFxVc2Vyc1xcXFxFbW1hIEx1Y2FzXFxcXE9uZURyaXZlIC0gS2Fuc2FzIFN0YXRlIFVuaXZlcnNpdHlcXFxcU2VuaW9yIFByb2plY3RcXFxcS1NVRnJlZVlvZ2FcXFxcS1N0YXRlRnJlZVlvZ2FcXFxca3N0YXRlZnJlZXlvZ2EuY2xpZW50XFxcXHZpdGUuY29uZmlnLmpzXCI7Y29uc3QgX192aXRlX2luamVjdGVkX29yaWdpbmFsX2ltcG9ydF9tZXRhX3VybCA9IFwiZmlsZTovLy9DOi9Vc2Vycy9FbW1hJTIwTHVjYXMvT25lRHJpdmUlMjAtJTIwS2Fuc2FzJTIwU3RhdGUlMjBVbml2ZXJzaXR5L1NlbmlvciUyMFByb2plY3QvS1NVRnJlZVlvZ2EvS1N0YXRlRnJlZVlvZ2Eva3N0YXRlZnJlZXlvZ2EuY2xpZW50L3ZpdGUuY29uZmlnLmpzXCI7aW1wb3J0IHsgZmlsZVVSTFRvUGF0aCwgVVJMIH0gZnJvbSAnbm9kZTp1cmwnO1xyXG5cclxuaW1wb3J0IHsgZGVmaW5lQ29uZmlnIH0gZnJvbSAndml0ZSc7XHJcbmltcG9ydCBwbHVnaW4gZnJvbSAnQHZpdGVqcy9wbHVnaW4tdnVlJztcclxuaW1wb3J0IGZzIGZyb20gJ2ZzJztcclxuaW1wb3J0IHBhdGggZnJvbSAncGF0aCc7XHJcbmltcG9ydCBjaGlsZF9wcm9jZXNzIGZyb20gJ2NoaWxkX3Byb2Nlc3MnO1xyXG5cclxuY29uc3QgYmFzZUZvbGRlciA9XHJcbiAgICBwcm9jZXNzLmVudi5BUFBEQVRBICE9PSB1bmRlZmluZWQgJiYgcHJvY2Vzcy5lbnYuQVBQREFUQSAhPT0gJydcclxuICAgICAgICA/IGAke3Byb2Nlc3MuZW52LkFQUERBVEF9L0FTUC5ORVQvaHR0cHNgXHJcbiAgICAgICAgOiBgJHtwcm9jZXNzLmVudi5IT01FfS8uYXNwbmV0L2h0dHBzYDtcclxuXHJcbmNvbnN0IGNlcnRpZmljYXRlQXJnID0gcHJvY2Vzcy5hcmd2Lm1hcChhcmcgPT4gYXJnLm1hdGNoKC8tLW5hbWU9KD88dmFsdWU+LispL2kpKS5maWx0ZXIoQm9vbGVhbilbMF07XHJcbmNvbnN0IGNlcnRpZmljYXRlTmFtZSA9IGNlcnRpZmljYXRlQXJnID8gY2VydGlmaWNhdGVBcmcuZ3JvdXBzLnZhbHVlIDogXCJrc3RhdGVmcmVleW9nYS5jbGllbnRcIjtcclxuXHJcbmlmICghY2VydGlmaWNhdGVOYW1lKSB7XHJcbiAgICBjb25zb2xlLmVycm9yKCdJbnZhbGlkIGNlcnRpZmljYXRlIG5hbWUuIFJ1biB0aGlzIHNjcmlwdCBpbiB0aGUgY29udGV4dCBvZiBhbiBucG0veWFybiBzY3JpcHQgb3IgcGFzcyAtLW5hbWU9PDxhcHA+PiBleHBsaWNpdGx5LicpXHJcbiAgICBwcm9jZXNzLmV4aXQoLTEpO1xyXG59XHJcblxyXG5jb25zdCBjZXJ0RmlsZVBhdGggPSBwYXRoLmpvaW4oYmFzZUZvbGRlciwgYCR7Y2VydGlmaWNhdGVOYW1lfS5wZW1gKTtcclxuY29uc3Qga2V5RmlsZVBhdGggPSBwYXRoLmpvaW4oYmFzZUZvbGRlciwgYCR7Y2VydGlmaWNhdGVOYW1lfS5rZXlgKTtcclxuXHJcbmlmICghZnMuZXhpc3RzU3luYyhjZXJ0RmlsZVBhdGgpIHx8ICFmcy5leGlzdHNTeW5jKGtleUZpbGVQYXRoKSkge1xyXG4gICAgaWYgKDAgIT09IGNoaWxkX3Byb2Nlc3Muc3Bhd25TeW5jKCdkb3RuZXQnLCBbXHJcbiAgICAgICAgJ2Rldi1jZXJ0cycsXHJcbiAgICAgICAgJ2h0dHBzJyxcclxuICAgICAgICAnLS1leHBvcnQtcGF0aCcsXHJcbiAgICAgICAgY2VydEZpbGVQYXRoLFxyXG4gICAgICAgICctLWZvcm1hdCcsXHJcbiAgICAgICAgJ1BlbScsXHJcbiAgICAgICAgJy0tbm8tcGFzc3dvcmQnLFxyXG4gICAgXSwgeyBzdGRpbzogJ2luaGVyaXQnLCB9KS5zdGF0dXMpIHtcclxuICAgICAgICB0aHJvdyBuZXcgRXJyb3IoXCJDb3VsZCBub3QgY3JlYXRlIGNlcnRpZmljYXRlLlwiKTtcclxuICAgIH1cclxufVxyXG5cclxuLy8gaHR0cHM6Ly92aXRlanMuZGV2L2NvbmZpZy9cclxuZXhwb3J0IGRlZmF1bHQgZGVmaW5lQ29uZmlnKHtcclxuICAgIHBsdWdpbnM6IFtwbHVnaW4oKV0sXHJcbiAgICByZXNvbHZlOiB7XHJcbiAgICAgICAgYWxpYXM6IHtcclxuICAgICAgICAgICAgJ0AnOiBmaWxlVVJMVG9QYXRoKG5ldyBVUkwoJy4vc3JjJywgaW1wb3J0Lm1ldGEudXJsKSlcclxuICAgICAgICB9XHJcbiAgICB9LFxyXG4gICAgc2VydmVyOiB7XHJcbiAgICAgICAgcHJveHk6IHtcclxuICAgICAgICAgICAgJ14vd2VhdGhlcmZvcmVjYXN0Jzoge1xyXG4gICAgICAgICAgICAgICAgdGFyZ2V0OiAnaHR0cHM6Ly9sb2NhbGhvc3Q6NTAwMS8nLFxyXG4gICAgICAgICAgICAgICAgc2VjdXJlOiBmYWxzZVxyXG4gICAgICAgICAgICB9XHJcbiAgICAgICAgfSxcclxuICAgICAgICBwb3J0OiA1MTczLFxyXG4gICAgICAgIGh0dHBzOiB7XHJcbiAgICAgICAgICAgIGtleTogZnMucmVhZEZpbGVTeW5jKGtleUZpbGVQYXRoKSxcclxuICAgICAgICAgICAgY2VydDogZnMucmVhZEZpbGVTeW5jKGNlcnRGaWxlUGF0aCksXHJcbiAgICAgICAgfVxyXG4gICAgfVxyXG59KVxyXG4iXSwKICAibWFwcGluZ3MiOiAiO0FBQWdqQixTQUFTLGVBQWUsV0FBVztBQUVubEIsU0FBUyxvQkFBb0I7QUFDN0IsT0FBTyxZQUFZO0FBQ25CLE9BQU8sUUFBUTtBQUNmLE9BQU8sVUFBVTtBQUNqQixPQUFPLG1CQUFtQjtBQU55VSxJQUFNLDJDQUEyQztBQVFwWixJQUFNLGFBQ0YsUUFBUSxJQUFJLFlBQVksVUFBYSxRQUFRLElBQUksWUFBWSxLQUN2RCxHQUFHLFFBQVEsSUFBSSxPQUFPLG1CQUN0QixHQUFHLFFBQVEsSUFBSSxJQUFJO0FBRTdCLElBQU0saUJBQWlCLFFBQVEsS0FBSyxJQUFJLFNBQU8sSUFBSSxNQUFNLHNCQUFzQixDQUFDLEVBQUUsT0FBTyxPQUFPLEVBQUUsQ0FBQztBQUNuRyxJQUFNLGtCQUFrQixpQkFBaUIsZUFBZSxPQUFPLFFBQVE7QUFFdkUsSUFBSSxDQUFDLGlCQUFpQjtBQUNsQixVQUFRLE1BQU0sbUhBQW1IO0FBQ2pJLFVBQVEsS0FBSyxFQUFFO0FBQ25CO0FBRUEsSUFBTSxlQUFlLEtBQUssS0FBSyxZQUFZLEdBQUcsZUFBZSxNQUFNO0FBQ25FLElBQU0sY0FBYyxLQUFLLEtBQUssWUFBWSxHQUFHLGVBQWUsTUFBTTtBQUVsRSxJQUFJLENBQUMsR0FBRyxXQUFXLFlBQVksS0FBSyxDQUFDLEdBQUcsV0FBVyxXQUFXLEdBQUc7QUFDN0QsTUFBSSxNQUFNLGNBQWMsVUFBVSxVQUFVO0FBQUEsSUFDeEM7QUFBQSxJQUNBO0FBQUEsSUFDQTtBQUFBLElBQ0E7QUFBQSxJQUNBO0FBQUEsSUFDQTtBQUFBLElBQ0E7QUFBQSxFQUNKLEdBQUcsRUFBRSxPQUFPLFVBQVcsQ0FBQyxFQUFFLFFBQVE7QUFDOUIsVUFBTSxJQUFJLE1BQU0sK0JBQStCO0FBQUEsRUFDbkQ7QUFDSjtBQUdBLElBQU8sc0JBQVEsYUFBYTtBQUFBLEVBQ3hCLFNBQVMsQ0FBQyxPQUFPLENBQUM7QUFBQSxFQUNsQixTQUFTO0FBQUEsSUFDTCxPQUFPO0FBQUEsTUFDSCxLQUFLLGNBQWMsSUFBSSxJQUFJLFNBQVMsd0NBQWUsQ0FBQztBQUFBLElBQ3hEO0FBQUEsRUFDSjtBQUFBLEVBQ0EsUUFBUTtBQUFBLElBQ0osT0FBTztBQUFBLE1BQ0gscUJBQXFCO0FBQUEsUUFDakIsUUFBUTtBQUFBLFFBQ1IsUUFBUTtBQUFBLE1BQ1o7QUFBQSxJQUNKO0FBQUEsSUFDQSxNQUFNO0FBQUEsSUFDTixPQUFPO0FBQUEsTUFDSCxLQUFLLEdBQUcsYUFBYSxXQUFXO0FBQUEsTUFDaEMsTUFBTSxHQUFHLGFBQWEsWUFBWTtBQUFBLElBQ3RDO0FBQUEsRUFDSjtBQUNKLENBQUM7IiwKICAibmFtZXMiOiBbXQp9Cg==
