# Models/NavigationModel.cs

**Mudanca:** PEQUENA | **+0** linhas | **-0** linhas

---

```diff
--- JANEIRO: Models/NavigationModel.cs
+++ ATUAL: Models/NavigationModel.cs
@@ -9,8 +9,10 @@
 
 namespace FrotiX.Models
     {
+
     public class NavigationModel : INavigationModel
         {
+
         public static readonly string Void = "javascript:void(0);";
         private const string Dash = "-";
         private const string Space = " ";
@@ -29,6 +31,7 @@
             }
 
         public SmartNavigation Full => BuildNavigation(seedOnly: false);
+
         public SmartNavigation Seed => BuildNavigation();
 
         private static SmartNavigation BuildNavigation(bool seedOnly = true)
```
