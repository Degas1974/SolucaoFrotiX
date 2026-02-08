# Mapeamento de Conexões e Dependências

## Estrutura Hierárquica

### Areas
#### Authorization
*   **Roles.cshtml.cs**
    *   **Entrada:** GET /Authorization/Roles
*   **Users.cshtml.cs**
    *   **Entrada:** GET /Authorization/Users
*   **Usuarios.cshtml.cs**
    *   **Entrada:** GET /Authorization/Usuarios

#### Identity
*   **ConfirmarSenha.cshtml.cs**
    *   **Entrada:** OnGetAsync, OnPostAsync
    *   **Saída:** _signInManager, HttpContext.SignOutAsync, Url.Content
*   **ConfirmEmail.cshtml.cs** (Account)
    *   **Entrada:** GET /Identity/Account/ConfirmEmail
    *   **Saída:** _userManager.FindByIdAsync, _userManager.ConfirmEmailAsync
*   **ConfirmEmailChange.cshtml.cs** (Account)
    *   **Entrada:** GET /Identity/Account/ConfirmEmailChange
    *   **Saída:** _userManager.ChangeEmailAsync, _signInManager.RefreshSignInAsync
*   **ForgotPassword.cshtml.cs** (Account)
    *   **Entrada:** GET/POST /Identity/Account/ForgotPassword
    *   **Saída:** _signInManager.SignOutAsync, _emailSender.SendEmailAsync
*   **ForgotPasswordConfirmation.cshtml.cs** (Account)
    *   **Entrada:** GET /Identity/Account/ForgotPasswordConfirmation
*   **Lockout.cshtml.cs** (Account)
    *   **Entrada:** GET/POST /Identity/Account/Lockout
    *   **Saída:** _signInManager.SignOutAsync, _signInManager.PasswordSignInAsync
*   **Login.cshtml.cs** (Account)
    *   **Entrada:** GET/POST /Identity/Account/Login
    *   **Saída:** _signInManager.PasswordSignInAsync
*   **LoginFrotiX.cshtml.cs** (Account)
    *   **Entrada:** GET/POST /Identity/Account/LoginFrotiX
    *   **Saída:** _signInManager.PasswordSignInAsync, _signInManager.GetExternalAuthenticationSchemesAsync
*   **Logout.cshtml.cs** (Account)
    *   **Entrada:** GET/POST /Identity/Account/Logout
    *   **Saída:** _signInManager.SignOutAsync
*   **Register.cshtml.cs** (Account)
    *   **Entrada:** GET/POST /Identity/Account/Register
    *   **Saída:** _userManager.CreateAsync, _signInManager.SignInAsync
*   **RegisterConfirmation.cshtml.cs** (Account)
    *   **Entrada:** GET /Identity/Account/RegisterConfirmation
    *   **Saída:** _userManager.FindByEmailAsync, _userManager.GenerateEmailConfirmationTokenAsync
*   **ResetPassword.cshtml.cs** (Account)
    *   **Entrada:** GET/POST /Identity/Account/ResetPassword
    *   **Saída:** _userManager.FindByEmailAsync, _userManager.ResetPasswordAsync
*   **ResetPasswordConfirmation.cshtml.cs** (Account)
    *   **Entrada:** GET /Identity/Account/ResetPasswordConfirmation

### Controllers
#### AbastecimentoController
*   **DashboardDados (GET)**
    *   **Entrada:** /api/Abastecimento/DashboardDados
    *   **Saída:** _context.EstatisticaAbastecimentoMensal, _unitOfWork.ViewAbastecimentos
*   **DashboardDadosPeriodo (GET)**
    *   **Entrada:** /api/Abastecimento/DashboardDadosPeriodo
    *   **Saída:** _unitOfWork.ViewAbastecimentos
*   **ImportarNovo (POST)**
    *   **Entrada:** /api/Abastecimento/ImportarNovo
    *   **Saída:** _unitOfWork.Abastecimento.Add, _unitOfWork.AbastecimentoPendente.Add
*   **ImportarDualInternal (Internal)**
    *   **Entrada:** Chamada interna
    *   **Saída:** Leitura Excel/CSV, _unitOfWork.Abastecimento.Add
*   **ListarPendencias (GET)**
    *   **Entrada:** /api/Abastecimento/ListarPendencias
    *   **Saída:** _unitOfWork.AbastecimentoPendente.GetAll
*   **ResolverPendencia (POST)**
    *   **Entrada:** /api/Abastecimento/ResolverPendencia
    *   **Saída:** _unitOfWork.Abastecimento.Add, _unitOfWork.AbastecimentoPendente.Update
