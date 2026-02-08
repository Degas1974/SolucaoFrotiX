# Infrastructure/CacheKeys.cs

**Mudanca:** MEDIA | **+4** linhas | **-3** linhas

---

```diff
--- JANEIRO: Infrastructure/CacheKeys.cs
+++ ATUAL: Infrastructure/CacheKeys.cs
@@ -4,10 +4,12 @@
     public static class CacheKeys
     {
 
-        public const string Motoristas = "upsert:motoristas";
+        public const string Motoristas = "lista:motoristas";
 
-        public const string Veiculos = "upsert:veiculos";
+        public const string Veiculos = "lista:veiculos";
 
-        public const string VeiculosReserva = "upsert:veiculosreserva";
+        public const string VeiculosManutencao = "lista:veiculos:manutencao";
+
+        public const string VeiculosReserva = "lista:veiculos:reserva";
     }
 }
```

### REMOVER do Janeiro

```csharp
        public const string Motoristas = "upsert:motoristas";
        public const string Veiculos = "upsert:veiculos";
        public const string VeiculosReserva = "upsert:veiculosreserva";
```


### ADICIONAR ao Janeiro

```csharp
        public const string Motoristas = "lista:motoristas";
        public const string Veiculos = "lista:veiculos";
        public const string VeiculosManutencao = "lista:veiculos:manutencao";
        public const string VeiculosReserva = "lista:veiculos:reserva";
```
