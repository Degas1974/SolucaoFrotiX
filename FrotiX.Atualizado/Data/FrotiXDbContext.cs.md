# Data/FrotiXDbContext.cs

**Mudanca:** PEQUENA | **+4** linhas | **-1** linhas

---

```diff
--- JANEIRO: Data/FrotiXDbContext.cs
+++ ATUAL: Data/FrotiXDbContext.cs
@@ -7,8 +7,10 @@
 
 namespace FrotiX.Data
 {
+
     public partial class FrotiXDbContext : DbContext
     {
+
         public FrotiXDbContext(DbContextOptions<FrotiXDbContext> options)
             : base(options)
         {
@@ -28,6 +30,11 @@
         }
 
         public DbSet<AlertasUsuario> AlertasUsuario
+        {
+            get; set;
+        }
+
+        public DbSet<LogErro> LogErros
         {
             get; set;
         }
@@ -724,8 +731,6 @@
             modelBuilder.Entity<ViewControleAcesso>().HasNoKey();
             modelBuilder.Entity<ViewGlosa>().HasNoKey();
             modelBuilder.Entity<ViewPatrimonioConferencia>().HasNoKey();
-
-            ApplyDecimalPrecisionDefaults(modelBuilder);
         }
     }
 }
```

### REMOVER do Janeiro

```csharp
            ApplyDecimalPrecisionDefaults(modelBuilder);
```


### ADICIONAR ao Janeiro

```csharp
        {
            get; set;
        }
        public DbSet<LogErro> LogErros
```
