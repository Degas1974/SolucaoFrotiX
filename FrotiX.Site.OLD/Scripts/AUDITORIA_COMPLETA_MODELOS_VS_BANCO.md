# AUDITORIA COMPLETA: Modelos C# vs Banco de Dados SQL

**Data:** 1771024965.2929552
**Escopo:** Modelos principais do sistema

---

## 📊 ESTATÍSTICAS GERAIS

- **Total de tabelas SQL:** 120
- **Total de modelos C#:** 155
- **Total de discrepâncias encontradas:** 761

---

## 🔍 ANÁLISE POR MODELO

### ⚠️ Abastecimento.cs

**Status:** ⚠️ 6 discrepância(s) encontrada(s)

#### 1. **Abastecimento**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Abastecimento Abastecimento`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 2. **Litros**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `double? (nullable=True)`
- **SQL:** `float (NOT NULL)`
- **Correção:** Alterar C# para: 

#### 3. **ValorUnitario**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `double? (nullable=True)`
- **SQL:** `float (NOT NULL)`
- **Correção:** Alterar C# para: 

#### 4. **DataHora**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `DateTime? (nullable=True)`
- **SQL:** `datetime (NOT NULL)`
- **Correção:** Alterar C# para: 

#### 5. **KmRodado**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `int? (nullable=True)`
- **SQL:** `int (NOT NULL)`
- **Correção:** Alterar C# para: 

#### 6. **Hodometro**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `int? (nullable=True)`
- **SQL:** `int (NOT NULL)`
- **Correção:** Alterar C# para: 

---

### ⚠️ AbastecimentoPendente.cs

**Status:** ⚠️ 2 discrepância(s) encontrada(s)

#### 1. **TipoPendencia**

- **Problema:** MaxLength incompatível
- **Severidade:** 🟡 ATENÇÃO
- **C#:** `[MaxLength(2000)]`
- **SQL:** `(50)`
- **Correção:** Alterar [MaxLength] para 50

#### 2. **CampoCorrecao**

- **Problema:** MaxLength incompatível
- **Severidade:** 🟡 ATENÇÃO
- **C#:** `[MaxLength(50)]`
- **SQL:** `(20)`
- **Correção:** Alterar [MaxLength] para 20

---

### ⚠️ AlertasFrotiX.cs

**Status:** ⚠️ 20 discrepância(s) encontrada(s)

#### 1. **Titulo**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `string? (nullable=True)`
- **SQL:** `nvarchar(200) (NOT NULL)`
- **Correção:** Alterar C# para: 

#### 2. **Descricao**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `string? (nullable=True)`
- **SQL:** `nvarchar(1000) (NOT NULL)`
- **Correção:** Alterar C# para: 

#### 3. **DataInsercao**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `DateTime? (nullable=True)`
- **SQL:** `datetime2 (NOT NULL)`
- **Correção:** Alterar C# para: 

#### 4. **UsuarioCriadorId**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `string? (nullable=True)`
- **SQL:** `nvarchar(450) (NOT NULL)`
- **Correção:** Alterar C# para: 

#### 5. **Monday**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `bool (nullable=False)`
- **SQL:** `bit (NULL)`
- **Correção:** Alterar C# para: ?

#### 6. **Tuesday**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `bool (nullable=False)`
- **SQL:** `bit (NULL)`
- **Correção:** Alterar C# para: ?

#### 7. **Wednesday**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `bool (nullable=False)`
- **SQL:** `bit (NULL)`
- **Correção:** Alterar C# para: ?

#### 8. **Thursday**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `bool (nullable=False)`
- **SQL:** `bit (NULL)`
- **Correção:** Alterar C# para: ?

#### 9. **Friday**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `bool (nullable=False)`
- **SQL:** `bit (NULL)`
- **Correção:** Alterar C# para: ?

#### 10. **Saturday**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `bool (nullable=False)`
- **SQL:** `bit (NULL)`
- **Correção:** Alterar C# para: ?

#### 11. **Sunday**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `bool (nullable=False)`
- **SQL:** `bit (NULL)`
- **Correção:** Alterar C# para: ?

#### 12. **DiasSemana**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `string (nullable=False)`
- **SQL:** `varchar(500) (NULL)`
- **Correção:** Alterar C# para: ?

#### 13. **AlertasUsuarioId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid AlertasUsuarioId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 14. **UsuarioId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string UsuarioId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 15. **Lido**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public bool Lido`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 16. **DataLeitura**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public DateTime? DataLeitura`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 17. **Notificado**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public bool Notificado`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 18. **Apagado**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public bool Apagado`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 19. **DataApagado**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public DateTime? DataApagado`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 20. **DataNotificacao**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public DateTime? DataNotificacao`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

---

### ⚠️ AlertasUsuario.cs

**Status:** ⚠️ 30 discrepância(s) encontrada(s)

#### 1. **Titulo**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? Titulo`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 2. **Descricao**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? Descricao`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 3. **TipoAlerta**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public TipoAlerta TipoAlerta`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 4. **Prioridade**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public PrioridadeAlerta Prioridade`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 5. **DataInsercao**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public DateTime? DataInsercao`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 6. **DataExibicao**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public DateTime? DataExibicao`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 7. **DataExpiracao**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public DateTime? DataExpiracao`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 8. **DataDesativacao**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public DateTime? DataDesativacao`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 9. **DesativadoPor**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? DesativadoPor`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 10. **MotivoDesativacao**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? MotivoDesativacao`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 11. **ViagemId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid? ViagemId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 12. **ManutencaoId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid? ManutencaoId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 13. **MotoristaId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid? MotoristaId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 14. **VeiculoId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid? VeiculoId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 15. **TipoExibicao**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public TipoExibicaoAlerta TipoExibicao`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 16. **HorarioExibicao**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public TimeSpan? HorarioExibicao`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 17. **UsuarioCriadorId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? UsuarioCriadorId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 18. **Ativo**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public bool Ativo`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 19. **Monday**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public bool Monday`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 20. **Tuesday**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public bool Tuesday`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 21. **Wednesday**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public bool Wednesday`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 22. **Thursday**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public bool Thursday`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 23. **Friday**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public bool Friday`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 24. **Saturday**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public bool Saturday`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 25. **Sunday**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public bool Sunday`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 26. **DiaMesRecorrencia**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int? DiaMesRecorrencia`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 27. **DatasSelecionadas**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? DatasSelecionadas`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 28. **RecorrenciaAlertaId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid? RecorrenciaAlertaId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 29. **DiasSemana**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string DiasSemana`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 30. **Apagado**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `bool (nullable=False)`
- **SQL:** `bit (NULL)`
- **Correção:** Alterar C# para: ?

---

### ⚠️ AnosDisponiveisAbastecimento.cs

**Status:** ⚠️ 2 discrepância(s) encontrada(s)

#### 1. **TotalAbastecimentos**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `int (nullable=False)`
- **SQL:** `int (NULL)`
- **Correção:** Alterar C# para: ?

#### 2. **DataAtualizacao**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `DateTime (nullable=False)`
- **SQL:** `datetime (NULL)`
- **Correção:** Alterar C# para: ?

---

### ⚠️ AspNetUsers.cs

**Status:** ⚠️ 2 discrepância(s) encontrada(s)

#### 1. **Id**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `string? (nullable=True)`
- **SQL:** `nvarchar(450) (NOT NULL)`
- **Correção:** Alterar C# para: 

#### 2. **AspNetUsers**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public AspNetUsers? AspNetUsers`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

---

### ⚠️ AtaRegistroPrecos.cs

**Status:** ⚠️ 12 discrepância(s) encontrada(s)

#### 1. **AtaRegistroPrecos**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public AtaRegistroPrecos AtaRegistroPrecos`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 2. **NumeroProcesso**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `string (nullable=False)`
- **SQL:** `varchar(50) (NULL)`
- **Correção:** Alterar C# para: ?

#### 3. **Objeto**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `string (nullable=False)`
- **SQL:** `varchar(max) (NULL)`
- **Correção:** Alterar C# para: ?

#### 4. **Status**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `bool (nullable=False)`
- **SQL:** `bit (NULL)`
- **Correção:** Alterar C# para: ?

#### 5. **FornecedorId**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `Guid (nullable=False)`
- **SQL:** `uniqueidentifier (NULL)`
- **Correção:** Alterar C# para: ?

#### 6. **RepactuacaoAtaId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid RepactuacaoAtaId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 7. **DataRepactuacao**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public DateTime? DataRepactuacao`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 8. **Descricao**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? Descricao`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 9. **ItemVeiculoAtaId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid ItemVeiculoAtaId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 10. **NumItem**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int? NumItem`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 11. **Quantidade**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int? Quantidade`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 12. **ValorUnitario**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public double? ValorUnitario`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

---

### ⚠️ CoberturaFolga.cs

**Status:** ⚠️ 27 discrepância(s) encontrada(s)

#### 1. **TipoServicoId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid TipoServicoId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 2. **NomeServico**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? NomeServico`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 3. **Descricao**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string Descricao`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 4. **TurnoId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid TurnoId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 5. **NomeTurno**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? NomeTurno`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 6. **HoraInicio**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public TimeSpan HoraInicio`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 7. **HoraFim**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public TimeSpan HoraFim`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 8. **AssociacaoId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid? AssociacaoId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 9. **MotoristaId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid MotoristaId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 10. **VeiculoId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid? VeiculoId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 11. **EscalaDiaId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid EscalaDiaId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 12. **DataEscala**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public DateTime DataEscala`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 13. **HoraIntervaloInicio**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public TimeSpan? HoraIntervaloInicio`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 14. **HoraIntervaloFim**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public TimeSpan? HoraIntervaloFim`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 15. **Lotacao**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? Lotacao`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 16. **NumeroSaidas**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int NumeroSaidas`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 17. **StatusMotorista**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string StatusMotorista`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 18. **RequisitanteId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid? RequisitanteId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 19. **FolgaId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid FolgaId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 20. **Tipo**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string Tipo`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 21. **FeriasId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid FeriasId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 22. **MotoristaSubId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid? MotoristaSubId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 23. **ObservacaoId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid ObservacaoId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 24. **Titulo**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? Titulo`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 25. **Prioridade**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string Prioridade`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 26. **ExibirDe**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public DateTime ExibirDe`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 27. **ExibirAte**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public DateTime ExibirAte`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

---

### ⚠️ Combustivel.cs

**Status:** ⚠️ 5 discrepância(s) encontrada(s)

#### 1. **Status**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `bool (nullable=False)`
- **SQL:** `bit (NULL)`
- **Correção:** Alterar C# para: ?

#### 2. **NotaFiscalId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid NotaFiscalId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 3. **Ano**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int Ano`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 4. **Mes**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int Mes`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 5. **PrecoMedio**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public double PrecoMedio`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

---

### ⚠️ Contrato.cs

**Status:** ⚠️ 28 discrepância(s) encontrada(s)

#### 1. **Contrato**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Contrato Contrato`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 2. **ContratoEncarregados**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `bool (nullable=False)`
- **SQL:** `bit (NULL)`
- **Correção:** Alterar C# para: ?

#### 3. **ContratoOperadores**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `bool (nullable=False)`
- **SQL:** `bit (NULL)`
- **Correção:** Alterar C# para: ?

#### 4. **ContratoMotoristas**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `bool (nullable=False)`
- **SQL:** `bit (NULL)`
- **Correção:** Alterar C# para: ?

#### 5. **ContratoLavadores**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `bool (nullable=False)`
- **SQL:** `bit (NULL)`
- **Correção:** Alterar C# para: ?

#### 6. **Status**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `bool (nullable=False)`
- **SQL:** `bit (NULL)`
- **Correção:** Alterar C# para: ?

#### 7. **FornecedorId**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `Guid (nullable=False)`
- **SQL:** `uniqueidentifier (NULL)`
- **Correção:** Alterar C# para: ?

#### 8. **NotaFiscalId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid NotaFiscalId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 9. **Ano**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int Ano`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 10. **Mes**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int Mes`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 11. **RepactuacaoContratoId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid RepactuacaoContratoId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 12. **Descricao**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? Descricao`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 13. **Percentual**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public double? Percentual`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 14. **AtualizaContrato**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public bool AtualizaContrato`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 15. **ItemVeiculoId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid ItemVeiculoId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 16. **NumItem**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int? NumItem`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 17. **Quantidade**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int? Quantidade`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 18. **ValorUnitario**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public double? ValorUnitario`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 19. **RepactuacaoTerceirizacaoId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid RepactuacaoTerceirizacaoId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 20. **ValorEncarregado**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public double? ValorEncarregado`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 21. **ValorOperador**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public double? ValorOperador`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 22. **ValorMotorista**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public double? ValorMotorista`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 23. **ValorLavador**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public double? ValorLavador`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 24. **QtdEncarregados**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int? QtdEncarregados`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 25. **QtdOperadores**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int? QtdOperadores`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 26. **QtdMotoristas**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int? QtdMotoristas`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 27. **QtdLavadores**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int? QtdLavadores`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 28. **RepactuacaoServicoId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid RepactuacaoServicoId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

---

### ⚠️ CorridasTaxiLeg.cs

**Status:** ⚠️ 5 discrepância(s) encontrada(s)

#### 1. **AbastecimentoId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid AbastecimentoId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 2. **VeiculoId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid VeiculoId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 3. **MotoristaId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid MotoristaId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 4. **CombustivelId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid CombustivelId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 5. **Glosa**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `bool (nullable=False)`
- **SQL:** `bit (NULL)`
- **Correção:** Alterar C# para: ?

---

### ⚠️ CustoMensalItensContrato.cs

**Status:** ⚠️ 43 discrepância(s) encontrada(s)

#### 1. **ContratoId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid ContratoId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 2. **Contrato**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Contrato Contrato`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 3. **NumeroContrato**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? NumeroContrato`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 4. **AnoContrato**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? AnoContrato`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 5. **Vigencia**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int? Vigencia`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 6. **Prorrogacao**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int? Prorrogacao`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 7. **AnoProcesso**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int? AnoProcesso`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 8. **NumeroProcesso**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? NumeroProcesso`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 9. **Objeto**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? Objeto`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 10. **TipoContrato**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? TipoContrato`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 11. **DataRepactuacao**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public DateTime? DataRepactuacao`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 12. **DataInicio**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public DateTime? DataInicio`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 13. **DataFim**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public DateTime? DataFim`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 14. **Valor**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public double? Valor`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 15. **ContratoEncarregados**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public bool ContratoEncarregados`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 16. **ContratoOperadores**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public bool ContratoOperadores`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 17. **ContratoMotoristas**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public bool ContratoMotoristas`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 18. **ContratoLavadores**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public bool ContratoLavadores`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 19. **CustoMensalEncarregado**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public double? CustoMensalEncarregado`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 20. **QuantidadeEncarregado**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int? QuantidadeEncarregado`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 21. **QuantidadeMotorista**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int? QuantidadeMotorista`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 22. **QuantidadeOperador**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int? QuantidadeOperador`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 23. **QuantidadeLavador**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int? QuantidadeLavador`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 24. **Status**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public bool Status`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 25. **FornecedorId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid FornecedorId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 26. **RepactuacaoContratoId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid RepactuacaoContratoId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 27. **Descricao**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? Descricao`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 28. **Percentual**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public double? Percentual`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 29. **AtualizaContrato**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public bool AtualizaContrato`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 30. **ItemVeiculoId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid ItemVeiculoId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 31. **NumItem**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int? NumItem`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 32. **Quantidade**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int? Quantidade`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 33. **ValorUnitario**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public double? ValorUnitario`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 34. **RepactuacaoTerceirizacaoId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid RepactuacaoTerceirizacaoId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 35. **ValorEncarregado**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public double? ValorEncarregado`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 36. **ValorOperador**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public double? ValorOperador`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 37. **ValorMotorista**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public double? ValorMotorista`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 38. **ValorLavador**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public double? ValorLavador`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 39. **QtdEncarregados**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int? QtdEncarregados`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 40. **QtdOperadores**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int? QtdOperadores`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 41. **QtdMotoristas**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int? QtdMotoristas`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 42. **QtdLavadores**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int? QtdLavadores`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 43. **RepactuacaoServicoId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid RepactuacaoServicoId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

---

### ⚠️ Empenho.cs

**Status:** ⚠️ 6 discrepância(s) encontrada(s)

#### 1. **Empenho**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Empenho Empenho`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 2. **NotaEmpenho**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `string? (nullable=True)`
- **SQL:** `varchar(12) (NOT NULL)`
- **Correção:** Alterar C# para: 

#### 3. **DataEmissao**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `DateTime? (nullable=True)`
- **SQL:** `date (NOT NULL)`
- **Correção:** Alterar C# para: 

#### 4. **AnoVigencia**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `int? (nullable=True)`
- **SQL:** `int (NOT NULL)`
- **Correção:** Alterar C# para: 

#### 5. **SaldoInicial**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `double? (nullable=True)`
- **SQL:** `float (NOT NULL)`
- **Correção:** Alterar C# para: 

#### 6. **SaldoFinal**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `double? (nullable=True)`
- **SQL:** `float (NOT NULL)`
- **Correção:** Alterar C# para: 

---

### ⚠️ EmpenhoMulta.cs

**Status:** ⚠️ 4 discrepância(s) encontrada(s)

#### 1. **EmpenhoMulta**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public EmpenhoMulta EmpenhoMulta`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 2. **NotaEmpenho**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `string? (nullable=True)`
- **SQL:** `varchar(50) (NOT NULL)`
- **Correção:** Alterar C# para: 

#### 3. **Status**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `bool (nullable=False)`
- **SQL:** `bit (NULL)`
- **Correção:** Alterar C# para: ?

#### 4. **OrgaoAutuanteId**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `Guid (nullable=False)`
- **SQL:** `uniqueidentifier (NULL)`
- **Correção:** Alterar C# para: ?

---

### ⚠️ Encarregado.cs

**Status:** ⚠️ 8 discrepância(s) encontrada(s)

#### 1. **Encarregado**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Encarregado? Encarregado`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 2. **NomeUsuarioAlteracao**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? NomeUsuarioAlteracao`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 3. **Nome**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `string? (nullable=True)`
- **SQL:** `nvarchar(100) (NOT NULL)`
- **Correção:** Alterar C# para: 

#### 4. **Ponto**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `string? (nullable=True)`
- **SQL:** `nvarchar(20) (NOT NULL)`
- **Correção:** Alterar C# para: 

#### 5. **DataNascimento**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `DateTime? (nullable=True)`
- **SQL:** `datetime2 (NOT NULL)`
- **Correção:** Alterar C# para: 

#### 6. **CPF**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `string? (nullable=True)`
- **SQL:** `nvarchar(20) (NOT NULL)`
- **Correção:** Alterar C# para: 

#### 7. **Celular01**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `string? (nullable=True)`
- **SQL:** `nvarchar(50) (NOT NULL)`
- **Correção:** Alterar C# para: 

#### 8. **ArquivoFoto**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public IFormFile? ArquivoFoto`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

---

### ⚠️ EncarregadoContrato.cs

**Status:** ⚠️ 1 discrepância(s) encontrada(s)

#### 1. **EncarregadoContrato**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public EncarregadoContrato? EncarregadoContrato`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

---

### ⚠️ EscalaDiaria.cs

**Status:** ⚠️ 21 discrepância(s) encontrada(s)

#### 1. **NomeServico**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? NomeServico`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 2. **Descricao**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string Descricao`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 3. **NomeTurno**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? NomeTurno`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 4. **MotoristaId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid MotoristaId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 5. **VeiculoId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid? VeiculoId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 6. **DataInicio**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public DateTime DataInicio`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 7. **DataFim**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public DateTime DataFim`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 8. **FolgaId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid FolgaId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 9. **Tipo**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string Tipo`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 10. **FeriasId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid FeriasId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 11. **MotoristaSubId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid? MotoristaSubId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 12. **CoberturaId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid CoberturaId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 13. **MotoristaFolgaId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid MotoristaFolgaId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 14. **MotoristaCoberturaId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid MotoristaCoberturaId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 15. **Motivo**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? Motivo`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 16. **StatusOriginal**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? StatusOriginal`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 17. **ObservacaoId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid ObservacaoId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 18. **Titulo**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? Titulo`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 19. **Prioridade**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string Prioridade`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 20. **ExibirDe**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public DateTime ExibirDe`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 21. **ExibirAte**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public DateTime ExibirAte`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

---

### ⚠️ EstatisticaAbastecimentoCategoria.cs

**Status:** ⚠️ 2 discrepância(s) encontrada(s)

#### 1. **TotalAbastecimentos**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `int (nullable=False)`
- **SQL:** `int (NULL)`
- **Correção:** Alterar C# para: ?

#### 2. **DataAtualizacao**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `DateTime (nullable=False)`
- **SQL:** `datetime (NULL)`
- **Correção:** Alterar C# para: ?

---

### ⚠️ EstatisticaAbastecimentoCombustivel.cs

**Status:** ⚠️ 2 discrepância(s) encontrada(s)

#### 1. **TotalAbastecimentos**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `int (nullable=False)`
- **SQL:** `int (NULL)`
- **Correção:** Alterar C# para: ?

#### 2. **DataAtualizacao**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `DateTime (nullable=False)`
- **SQL:** `datetime (NULL)`
- **Correção:** Alterar C# para: ?

---

### ⚠️ EstatisticaAbastecimentoMensal.cs

**Status:** ⚠️ 2 discrepância(s) encontrada(s)

#### 1. **TotalAbastecimentos**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `int (nullable=False)`
- **SQL:** `int (NULL)`
- **Correção:** Alterar C# para: ?

#### 2. **DataAtualizacao**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `DateTime (nullable=False)`
- **SQL:** `datetime (NULL)`
- **Correção:** Alterar C# para: ?

---

### ⚠️ EstatisticaAbastecimentoTipoVeiculo.cs

**Status:** ⚠️ 2 discrepância(s) encontrada(s)

#### 1. **TotalAbastecimentos**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `int (nullable=False)`
- **SQL:** `int (NULL)`
- **Correção:** Alterar C# para: ?

#### 2. **DataAtualizacao**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `DateTime (nullable=False)`
- **SQL:** `datetime (NULL)`
- **Correção:** Alterar C# para: ?

---

### ⚠️ EstatisticaAbastecimentoVeiculo.cs

**Status:** ⚠️ 2 discrepância(s) encontrada(s)

#### 1. **TotalAbastecimentos**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `int (nullable=False)`
- **SQL:** `int (NULL)`
- **Correção:** Alterar C# para: ?

#### 2. **DataAtualizacao**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `DateTime (nullable=False)`
- **SQL:** `datetime (NULL)`
- **Correção:** Alterar C# para: ?

---

### ⚠️ EstatisticaAbastecimentoVeiculoMensal.cs

**Status:** ⚠️ 2 discrepância(s) encontrada(s)

#### 1. **TotalAbastecimentos**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `int (nullable=False)`
- **SQL:** `int (NULL)`
- **Correção:** Alterar C# para: ?

#### 2. **DataAtualizacao**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `DateTime (nullable=False)`
- **SQL:** `datetime (NULL)`
- **Correção:** Alterar C# para: ?

---

### ⚠️ EstatisticaGeralMensal.cs

**Status:** ⚠️ 13 discrepância(s) encontrada(s)

#### 1. **TotalMotoristas**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `int (nullable=False)`
- **SQL:** `int (NULL)`
- **Correção:** Alterar C# para: ?

#### 2. **MotoristasAtivos**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `int (nullable=False)`
- **SQL:** `int (NULL)`
- **Correção:** Alterar C# para: ?

#### 3. **MotoristasInativos**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `int (nullable=False)`
- **SQL:** `int (NULL)`
- **Correção:** Alterar C# para: ?

#### 4. **Efetivos**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `int (nullable=False)`
- **SQL:** `int (NULL)`
- **Correção:** Alterar C# para: ?

#### 5. **Feristas**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `int (nullable=False)`
- **SQL:** `int (NULL)`
- **Correção:** Alterar C# para: ?

#### 6. **Cobertura**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `int (nullable=False)`
- **SQL:** `int (NULL)`
- **Correção:** Alterar C# para: ?

#### 7. **TotalViagens**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `int (nullable=False)`
- **SQL:** `int (NULL)`
- **Correção:** Alterar C# para: ?

#### 8. **KmTotal**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `decimal (nullable=False)`
- **SQL:** `decimal(18, 2) (NULL)`
- **Correção:** Alterar C# para: ?

#### 9. **HorasTotais**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `decimal (nullable=False)`
- **SQL:** `decimal(18, 2) (NULL)`
- **Correção:** Alterar C# para: ?

#### 10. **TotalMultas**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `int (nullable=False)`
- **SQL:** `int (NULL)`
- **Correção:** Alterar C# para: ?

#### 11. **ValorTotalMultas**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `decimal (nullable=False)`
- **SQL:** `decimal(18, 2) (NULL)`
- **Correção:** Alterar C# para: ?

#### 12. **TotalAbastecimentos**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `int (nullable=False)`
- **SQL:** `int (NULL)`
- **Correção:** Alterar C# para: ?

#### 13. **DataAtualizacao**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `DateTime (nullable=False)`
- **SQL:** `datetime (NULL)`
- **Correção:** Alterar C# para: ?

---

### ⚠️ EstatisticaMotoristasMensal.cs

**Status:** ⚠️ 9 discrepância(s) encontrada(s)

#### 1. **TotalViagens**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `int (nullable=False)`
- **SQL:** `int (NULL)`
- **Correção:** Alterar C# para: ?

#### 2. **KmTotal**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `decimal (nullable=False)`
- **SQL:** `decimal(18, 2) (NULL)`
- **Correção:** Alterar C# para: ?

#### 3. **MinutosTotais**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `int (nullable=False)`
- **SQL:** `int (NULL)`
- **Correção:** Alterar C# para: ?

#### 4. **TotalMultas**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `int (nullable=False)`
- **SQL:** `int (NULL)`
- **Correção:** Alterar C# para: ?

#### 5. **ValorTotalMultas**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `decimal (nullable=False)`
- **SQL:** `decimal(18, 2) (NULL)`
- **Correção:** Alterar C# para: ?

#### 6. **TotalAbastecimentos**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `int (nullable=False)`
- **SQL:** `int (NULL)`
- **Correção:** Alterar C# para: ?

#### 7. **LitrosTotais**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `decimal (nullable=False)`
- **SQL:** `decimal(18, 2) (NULL)`
- **Correção:** Alterar C# para: ?

#### 8. **ValorTotalAbastecimentos**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `decimal (nullable=False)`
- **SQL:** `decimal(18, 2) (NULL)`
- **Correção:** Alterar C# para: ?

#### 9. **DataAtualizacao**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `DateTime (nullable=False)`
- **SQL:** `datetime (NULL)`
- **Correção:** Alterar C# para: ?

---

### ⚠️ Evento.cs

**Status:** ⚠️ 2 discrepância(s) encontrada(s)

#### 1. **QtdParticipantes**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `int? (nullable=True)`
- **SQL:** `int (NOT NULL)`
- **Correção:** Alterar C# para: 

#### 2. **Evento**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Evento Evento`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

---

### ⚠️ EvolucaoViagensDiaria.cs

**Status:** ⚠️ 4 discrepância(s) encontrada(s)

#### 1. **TotalViagens**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `int (nullable=False)`
- **SQL:** `int (NULL)`
- **Correção:** Alterar C# para: ?

#### 2. **KmTotal**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `decimal (nullable=False)`
- **SQL:** `decimal(18, 2) (NULL)`
- **Correção:** Alterar C# para: ?

#### 3. **MinutosTotais**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `int (nullable=False)`
- **SQL:** `int (NULL)`
- **Correção:** Alterar C# para: ?

#### 4. **DataAtualizacao**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `DateTime (nullable=False)`
- **SQL:** `datetime (NULL)`
- **Correção:** Alterar C# para: ?

---

### ⚠️ Ferias.cs

**Status:** ⚠️ 29 discrepância(s) encontrada(s)

#### 1. **TipoServicoId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid TipoServicoId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 2. **NomeServico**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? NomeServico`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 3. **Descricao**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string Descricao`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 4. **TurnoId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid TurnoId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 5. **NomeTurno**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? NomeTurno`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 6. **HoraInicio**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public TimeSpan HoraInicio`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 7. **HoraFim**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public TimeSpan HoraFim`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 8. **AssociacaoId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid? AssociacaoId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 9. **VeiculoId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid? VeiculoId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 10. **EscalaDiaId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid EscalaDiaId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 11. **DataEscala**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public DateTime DataEscala`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 12. **HoraIntervaloInicio**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public TimeSpan? HoraIntervaloInicio`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 13. **HoraIntervaloFim**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public TimeSpan? HoraIntervaloFim`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 14. **Lotacao**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? Lotacao`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 15. **NumeroSaidas**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int NumeroSaidas`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 16. **StatusMotorista**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string StatusMotorista`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 17. **RequisitanteId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid? RequisitanteId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 18. **FolgaId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid FolgaId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 19. **Tipo**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string Tipo`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 20. **CoberturaId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid CoberturaId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 21. **MotoristaFolgaId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid MotoristaFolgaId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 22. **MotoristaCoberturaId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid MotoristaCoberturaId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 23. **Motivo**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? Motivo`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 24. **StatusOriginal**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? StatusOriginal`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 25. **ObservacaoId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid ObservacaoId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 26. **Titulo**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? Titulo`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 27. **Prioridade**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string Prioridade`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 28. **ExibirDe**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public DateTime ExibirDe`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 29. **ExibirAte**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public DateTime ExibirAte`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

---

### ⚠️ FolgaRecesso.cs

**Status:** ⚠️ 29 discrepância(s) encontrada(s)

#### 1. **TipoServicoId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid TipoServicoId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 2. **NomeServico**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? NomeServico`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 3. **Descricao**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string Descricao`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 4. **TurnoId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid TurnoId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 5. **NomeTurno**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? NomeTurno`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 6. **HoraInicio**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public TimeSpan HoraInicio`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 7. **HoraFim**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public TimeSpan HoraFim`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 8. **AssociacaoId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid? AssociacaoId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 9. **VeiculoId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid? VeiculoId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 10. **EscalaDiaId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid EscalaDiaId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 11. **DataEscala**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public DateTime DataEscala`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 12. **HoraIntervaloInicio**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public TimeSpan? HoraIntervaloInicio`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 13. **HoraIntervaloFim**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public TimeSpan? HoraIntervaloFim`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 14. **Lotacao**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? Lotacao`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 15. **NumeroSaidas**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int NumeroSaidas`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 16. **StatusMotorista**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string StatusMotorista`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 17. **RequisitanteId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid? RequisitanteId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 18. **FeriasId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid FeriasId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 19. **MotoristaSubId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid? MotoristaSubId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 20. **CoberturaId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid CoberturaId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 21. **MotoristaFolgaId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid MotoristaFolgaId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 22. **MotoristaCoberturaId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid MotoristaCoberturaId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 23. **Motivo**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? Motivo`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 24. **StatusOriginal**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? StatusOriginal`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 25. **ObservacaoId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid ObservacaoId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 26. **Titulo**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? Titulo`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 27. **Prioridade**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string Prioridade`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 28. **ExibirDe**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public DateTime ExibirDe`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 29. **ExibirAte**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public DateTime ExibirAte`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

---

### ⚠️ Fornecedor.cs

**Status:** ⚠️ 6 discrepância(s) encontrada(s)

#### 1. **FornecedorId**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `Guid (nullable=False)`
- **SQL:** `uniqueidentifier (NULL)`
- **Correção:** Alterar C# para: ?

#### 2. **DescricaoFornecedor**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `string (nullable=False)`
- **SQL:** `varchar(100) (NULL)`
- **Correção:** Alterar C# para: ?

#### 3. **CNPJ**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `string (nullable=False)`
- **SQL:** `varchar(50) (NULL)`
- **Correção:** Alterar C# para: ?

#### 4. **Contato01**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `string (nullable=False)`
- **SQL:** `varchar(100) (NULL)`
- **Correção:** Alterar C# para: ?

#### 5. **Telefone01**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `string (nullable=False)`
- **SQL:** `varchar(50) (NULL)`
- **Correção:** Alterar C# para: ?

#### 6. **Status**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `bool (nullable=False)`
- **SQL:** `bit (NULL)`
- **Correção:** Alterar C# para: ?

---

### ⚠️ HeatmapAbastecimentoMensal.cs

**Status:** ⚠️ 2 discrepância(s) encontrada(s)

#### 1. **TotalAbastecimentos**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `int (nullable=False)`
- **SQL:** `int (NULL)`
- **Correção:** Alterar C# para: ?

#### 2. **DataAtualizacao**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `DateTime (nullable=False)`
- **SQL:** `datetime (NULL)`
- **Correção:** Alterar C# para: ?

---

### ⚠️ HeatmapViagensMensal.cs

**Status:** ⚠️ 2 discrepância(s) encontrada(s)

#### 1. **TotalViagens**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `int (nullable=False)`
- **SQL:** `int (NULL)`
- **Correção:** Alterar C# para: ?

#### 2. **DataAtualizacao**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `DateTime (nullable=False)`
- **SQL:** `datetime (NULL)`
- **Correção:** Alterar C# para: ?

---

### ⚠️ ItensManutencao.cs

**Status:** ⚠️ 2 discrepância(s) encontrada(s)

#### 1. **ManutencaoId**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `Guid? (nullable=True)`
- **SQL:** `uniqueidentifier (NOT NULL)`
- **Correção:** Alterar C# para: 

#### 2. **NumOS**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string NumOS`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

---

### ⚠️ Lavador.cs

**Status:** ⚠️ 4 discrepância(s) encontrada(s)

#### 1. **Lavador**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Lavador Lavador`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 2. **NomeUsuarioAlteracao**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string NomeUsuarioAlteracao`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 3. **Status**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `bool (nullable=False)`
- **SQL:** `bit (NULL)`
- **Correção:** Alterar C# para: ?

#### 4. **ArquivoFoto**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public IFormFile? ArquivoFoto`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

---

### ⚠️ LavadorContrato.cs

**Status:** ⚠️ 1 discrepância(s) encontrada(s)

#### 1. **LavadorContrato**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public LavadorContrato LavadorContrato`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

---

### ⚠️ Lavagem.cs

**Status:** ⚠️ 2 discrepância(s) encontrada(s)

#### 1. **VeiculoId**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `Guid (nullable=False)`
- **SQL:** `uniqueidentifier (NULL)`
- **Correção:** Alterar C# para: ?

#### 2. **MotoristaId**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `Guid (nullable=False)`
- **SQL:** `uniqueidentifier (NULL)`
- **Correção:** Alterar C# para: ?

---

### ⚠️ LogErro.cs

**Status:** ⚠️ 10 discrepância(s) encontrada(s)

#### 1. **LogErroId**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `long (nullable=False)`
- **SQL:** `bigint (NULL)`
- **Correção:** Alterar C# para: ?

#### 2. **Origem**

- **Problema:** MaxLength incompatível
- **Severidade:** 🟡 ATENÇÃO
- **C#:** `[MaxLength(50)]`
- **SQL:** `(20)`
- **Correção:** Alterar [MaxLength] para 20

#### 3. **Categoria**

- **Problema:** MaxLength incompatível
- **Severidade:** 🟡 ATENÇÃO
- **C#:** `[MaxLength(20)]`
- **SQL:** `(100)`
- **Correção:** Alterar [MaxLength] para 100

#### 4. **Arquivo**

- **Problema:** MaxLength incompatível
- **Severidade:** 🟡 ATENÇÃO
- **C#:** `[MaxLength(203)]`
- **SQL:** `(500)`
- **Correção:** Alterar [MaxLength] para 500

#### 5. **Metodo**

- **Problema:** MaxLength incompatível
- **Severidade:** 🟡 ATENÇÃO
- **C#:** `[MaxLength(500)]`
- **SQL:** `(200)`
- **Correção:** Alterar [MaxLength] para 200

#### 6. **HttpMethod**

- **Problema:** MaxLength incompatível
- **Severidade:** 🟡 ATENÇÃO
- **C#:** `[MaxLength(1000)]`
- **SQL:** `(10)`
- **Correção:** Alterar [MaxLength] para 10

#### 7. **UserAgent**

- **Problema:** MaxLength incompatível
- **Severidade:** 🟡 ATENÇÃO
- **C#:** `[MaxLength(10)]`
- **SQL:** `(500)`
- **Correção:** Alterar [MaxLength] para 500

#### 8. **IpAddress**

- **Problema:** MaxLength incompatível
- **Severidade:** 🟡 ATENÇÃO
- **C#:** `[MaxLength(500)]`
- **SQL:** `(45)`
- **Correção:** Alterar [MaxLength] para 45

#### 9. **Usuario**

- **Problema:** MaxLength incompatível
- **Severidade:** 🟡 ATENÇÃO
- **C#:** `[MaxLength(45)]`
- **SQL:** `(100)`
- **Correção:** Alterar [MaxLength] para 100

#### 10. **CriadoEm**

- **Problema:** MaxLength incompatível
- **Severidade:** 🟡 ATENÇÃO
- **C#:** `[MaxLength(64)]`
- **SQL:** `(3)`
- **Correção:** Alterar [MaxLength] para 3

---

### ⚠️ LotacaoMotorista.cs

**Status:** ⚠️ 4 discrepância(s) encontrada(s)

#### 1. **MotoristaId**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `Guid (nullable=False)`
- **SQL:** `uniqueidentifier (NULL)`
- **Correção:** Alterar C# para: ?

#### 2. **MotoristaCoberturaId**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `Guid (nullable=False)`
- **SQL:** `uniqueidentifier (NULL)`
- **Correção:** Alterar C# para: ?

#### 3. **UnidadeId**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `Guid (nullable=False)`
- **SQL:** `uniqueidentifier (NULL)`
- **Correção:** Alterar C# para: ?

#### 4. **Lotado**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `bool (nullable=False)`
- **SQL:** `bit (NULL)`
- **Correção:** Alterar C# para: ?

---

### ⚠️ Manutencao.cs

**Status:** ⚠️ 5 discrepância(s) encontrada(s)

#### 1. **DataSolicitacao**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `DateTime? (nullable=True)`
- **SQL:** `datetime (NOT NULL)`
- **Correção:** Alterar C# para: 

#### 2. **ManutencaoPreventiva**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `bool (nullable=False)`
- **SQL:** `bit (NULL)`
- **Correção:** Alterar C# para: ?

#### 3. **NumOS**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `string? (nullable=True)`
- **SQL:** `varchar(50) (NOT NULL)`
- **Correção:** Alterar C# para: 

#### 4. **ReservaEnviado**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `bool (nullable=False)`
- **SQL:** `bit (NULL)`
- **Correção:** Alterar C# para: ?

#### 5. **Manutencao**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Manutencao? Manutencao`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

---

### ⚠️ MarcaVeiculo.cs

**Status:** ⚠️ 2 discrepância(s) encontrada(s)

#### 1. **DescricaoMarca**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `string? (nullable=True)`
- **SQL:** `varchar(50) (NOT NULL)`
- **Correção:** Alterar C# para: 

#### 2. **Status**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `bool (nullable=False)`
- **SQL:** `bit (NULL)`
- **Correção:** Alterar C# para: ?

---

### ⚠️ MediaCombustivel.cs

**Status:** ⚠️ 3 discrepância(s) encontrada(s)

#### 1. **Descricao**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string Descricao`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 2. **Status**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public bool Status`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 3. **PrecoMedio**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `double (nullable=False)`
- **SQL:** `float (NULL)`
- **Correção:** Alterar C# para: ?

---

### ⚠️ Motorista.cs

**Status:** ⚠️ 6 discrepância(s) encontrada(s)

#### 1. **Motorista**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Motorista? Motorista`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 2. **NomeUsuarioAlteracao**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? NomeUsuarioAlteracao`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 3. **Nome**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `string? (nullable=True)`
- **SQL:** `varchar(100) (NOT NULL)`
- **Correção:** Alterar C# para: 

#### 4. **Ponto**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `string? (nullable=True)`
- **SQL:** `varchar(50) (NOT NULL)`
- **Correção:** Alterar C# para: 

#### 5. **Status**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `bool (nullable=False)`
- **SQL:** `bit (NULL)`
- **Correção:** Alterar C# para: ?

#### 6. **ArquivoFoto**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public IFormFile? ArquivoFoto`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

---

### ⚠️ MotoristaContrato.cs

**Status:** ⚠️ 1 discrepância(s) encontrada(s)

#### 1. **MotoristaContrato**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public MotoristaContrato? MotoristaContrato`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

---

### ⚠️ MovimentacaoEmpenho.cs

**Status:** ⚠️ 2 discrepância(s) encontrada(s)

#### 1. **MovimentacaoEmpenho**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public MovimentacaoEmpenho? MovimentacaoEmpenho`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 2. **EmpenhoId**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `Guid (nullable=False)`
- **SQL:** `uniqueidentifier (NULL)`
- **Correção:** Alterar C# para: ?

---

### ⚠️ MovimentacaoEmpenhoMulta.cs

**Status:** ⚠️ 3 discrepância(s) encontrada(s)

#### 1. **MovimentacaoEmpenhoMulta**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public MovimentacaoEmpenhoMulta? MovimentacaoEmpenhoMulta`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 2. **MultaId**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `Guid (nullable=False)`
- **SQL:** `uniqueidentifier (NULL)`
- **Correção:** Alterar C# para: ?

#### 3. **EmpenhoMultaId**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `Guid (nullable=False)`
- **SQL:** `uniqueidentifier (NULL)`
- **Correção:** Alterar C# para: ?

---

### ⚠️ MovimentacaoPatrimonio.cs

**Status:** ⚠️ 14 discrepância(s) encontrada(s)

#### 1. **MovimentacaoPatrimonio**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public MovimentacaoPatrimonio? MovimentacaoPatrimonio`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 2. **PatrimonioId**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `Guid? (nullable=True)`
- **SQL:** `uniqueidentifier (NOT NULL)`
- **Correção:** Alterar C# para: 

#### 3. **SecaoOrigemId**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `Guid? (nullable=True)`
- **SQL:** `uniqueidentifier (NOT NULL)`
- **Correção:** Alterar C# para: 

#### 4. **SetorOrigemId**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `Guid? (nullable=True)`
- **SQL:** `uniqueidentifier (NOT NULL)`
- **Correção:** Alterar C# para: 

#### 5. **SecaoDestinoId**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `Guid? (nullable=True)`
- **SQL:** `uniqueidentifier (NOT NULL)`
- **Correção:** Alterar C# para: 

#### 6. **SetorDestinoId**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `Guid? (nullable=True)`
- **SQL:** `uniqueidentifier (NOT NULL)`
- **Correção:** Alterar C# para: 

#### 7. **NomeUsuarioAlteracao**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? NomeUsuarioAlteracao`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 8. **PatrimonioNome**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? PatrimonioNome`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 9. **SetorOrigemNome**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? SetorOrigemNome`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 10. **SecaoOrigemNome**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? SecaoOrigemNome`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 11. **SetorDestinoNome**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? SetorDestinoNome`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 12. **SecaoDestinoNome**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? SecaoDestinoNome`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 13. **DataMovimentacao**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `DateTime? (nullable=True)`
- **SQL:** `datetime (NOT NULL)`
- **Correção:** Alterar C# para: 

#### 14. **ResponsavelMovimentacao**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `string? (nullable=True)`
- **SQL:** `nvarchar(450) (NOT NULL)`
- **Correção:** Alterar C# para: 

---

### ⚠️ Multa.cs

**Status:** ⚠️ 6 discrepância(s) encontrada(s)

#### 1. **Multa**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Multa? Multa`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 2. **Data**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `DateTime? (nullable=True)`
- **SQL:** `datetime (NOT NULL)`
- **Correção:** Alterar C# para: 

#### 3. **Hora**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `DateTime? (nullable=True)`
- **SQL:** `datetime (NOT NULL)`
- **Correção:** Alterar C# para: 

#### 4. **Localizacao**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `string? (nullable=True)`
- **SQL:** `varchar(200) (NOT NULL)`
- **Correção:** Alterar C# para: 

#### 5. **Paga**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `bool? (nullable=True)`
- **SQL:** `bit (NOT NULL)`
- **Correção:** Alterar C# para: 

#### 6. **EnviadaSecle**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `bool? (nullable=True)`
- **SQL:** `bit (NOT NULL)`
- **Correção:** Alterar C# para: 

---

### ⚠️ NotaFiscal.cs

**Status:** ⚠️ 2 discrepância(s) encontrada(s)

#### 1. **NotaFiscal**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public NotaFiscal? NotaFiscal`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 2. **MediaGasolina**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public double? MediaGasolina`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

---

### ⚠️ ObservacoesEscala.cs

**Status:** ⚠️ 28 discrepância(s) encontrada(s)

#### 1. **TipoServicoId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid TipoServicoId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 2. **NomeServico**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? NomeServico`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 3. **TurnoId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid TurnoId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 4. **NomeTurno**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? NomeTurno`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 5. **HoraInicio**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public TimeSpan HoraInicio`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 6. **HoraFim**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public TimeSpan HoraFim`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 7. **AssociacaoId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid? AssociacaoId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 8. **MotoristaId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid MotoristaId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 9. **VeiculoId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid? VeiculoId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 10. **DataInicio**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public DateTime DataInicio`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 11. **DataFim**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public DateTime DataFim`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 12. **Observacoes**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? Observacoes`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 13. **EscalaDiaId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid EscalaDiaId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 14. **HoraIntervaloInicio**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public TimeSpan? HoraIntervaloInicio`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 15. **HoraIntervaloFim**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public TimeSpan? HoraIntervaloFim`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 16. **Lotacao**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? Lotacao`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 17. **NumeroSaidas**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int NumeroSaidas`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 18. **StatusMotorista**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string StatusMotorista`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 19. **RequisitanteId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid? RequisitanteId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 20. **FolgaId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid FolgaId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 21. **Tipo**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string Tipo`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 22. **FeriasId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid FeriasId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 23. **MotoristaSubId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid? MotoristaSubId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 24. **CoberturaId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid CoberturaId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 25. **MotoristaFolgaId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid MotoristaFolgaId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 26. **MotoristaCoberturaId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid MotoristaCoberturaId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 27. **Motivo**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? Motivo`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 28. **StatusOriginal**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? StatusOriginal`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

---

### ⚠️ OcorrenciaViagem.cs

**Status:** ⚠️ 1 discrepância(s) encontrada(s)

#### 1. **Solucao**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `string (nullable=False)`
- **SQL:** `varchar(500) (NULL)`
- **Correção:** Alterar C# para: ?

---

### ⚠️ Operador.cs

**Status:** ⚠️ 5 discrepância(s) encontrada(s)

#### 1. **ContratoId**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `Guid (nullable=False)`
- **SQL:** `uniqueidentifier (NULL)`
- **Correção:** Alterar C# para: ?

#### 2. **Operador**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Operador? Operador`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 3. **NomeUsuarioAlteracao**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? NomeUsuarioAlteracao`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 4. **Status**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `bool (nullable=False)`
- **SQL:** `bit (NULL)`
- **Correção:** Alterar C# para: ?

#### 5. **ArquivoFoto**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public IFormFile? ArquivoFoto`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

---

### ⚠️ OperadorContrato.cs

**Status:** ⚠️ 1 discrepância(s) encontrada(s)

#### 1. **OperadorContrato**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public OperadorContrato? OperadorContrato`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

---

### ⚠️ OrgaoAutuante.cs

**Status:** ⚠️ 1 discrepância(s) encontrada(s)

#### 1. **Sigla**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `string? (nullable=True)`
- **SQL:** `varchar(50) (NOT NULL)`
- **Correção:** Alterar C# para: 

---

### ⚠️ Patrimonio.cs

**Status:** ⚠️ 6 discrepância(s) encontrada(s)

#### 1. **Patrimonio**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Patrimonio? Patrimonio`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 2. **NPR**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `string? (nullable=True)`
- **SQL:** `varchar(50) (NOT NULL)`
- **Correção:** Alterar C# para: 

#### 3. **Descricao**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `string? (nullable=True)`
- **SQL:** `varchar(100) (NOT NULL)`
- **Correção:** Alterar C# para: 

#### 4. **LocalizacaoAtual**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `string? (nullable=True)`
- **SQL:** `varchar(150) (NOT NULL)`
- **Correção:** Alterar C# para: 

#### 5. **Situacao**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `string? (nullable=True)`
- **SQL:** `varchar(50) (NOT NULL)`
- **Correção:** Alterar C# para: 

#### 6. **Status**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `bool (nullable=False)`
- **SQL:** `bit (NULL)`
- **Correção:** Alterar C# para: ?

---

### ⚠️ PlacaBronze.cs

**Status:** ⚠️ 3 discrepância(s) encontrada(s)

#### 1. **PlacaBronze**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public PlacaBronze? PlacaBronze`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 2. **VeiculoId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid VeiculoId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 3. **Status**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `bool (nullable=False)`
- **SQL:** `bit (NULL)`
- **Correção:** Alterar C# para: ?

---

### ⚠️ RankingMotoristasMensal.cs

**Status:** ⚠️ 7 discrepância(s) encontrada(s)

#### 1. **NomeMotorista**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `string (nullable=False)`
- **SQL:** `nvarchar(200) (NULL)`
- **Correção:** Alterar C# para: ?

#### 2. **TipoMotorista**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `string (nullable=False)`
- **SQL:** `nvarchar(50) (NULL)`
- **Correção:** Alterar C# para: ?

#### 3. **ValorPrincipal**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `decimal (nullable=False)`
- **SQL:** `decimal(18, 2) (NULL)`
- **Correção:** Alterar C# para: ?

#### 4. **ValorSecundario**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `decimal (nullable=False)`
- **SQL:** `decimal(18, 2) (NULL)`
- **Correção:** Alterar C# para: ?

#### 5. **ValorTerciario**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `decimal (nullable=False)`
- **SQL:** `decimal(18, 2) (NULL)`
- **Correção:** Alterar C# para: ?

#### 6. **ValorQuaternario**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `int (nullable=False)`
- **SQL:** `int (NULL)`
- **Correção:** Alterar C# para: ?

#### 7. **DataAtualizacao**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `DateTime (nullable=False)`
- **SQL:** `datetime (NULL)`
- **Correção:** Alterar C# para: ?

---

### ⚠️ Recurso.cs

**Status:** ⚠️ 1 discrepância(s) encontrada(s)

#### 1. **Recurso**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Recurso? Recurso`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

---

### ⚠️ RegistroCupomAbastecimento.cs

**Status:** ⚠️ 3 discrepância(s) encontrada(s)

#### 1. **RegistroCupomAbastecimento**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public RegistroCupomAbastecimento? RegistroCupomAbastecimento`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 2. **DataRegistro**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `DateTime? (nullable=True)`
- **SQL:** `date (NOT NULL)`
- **Correção:** Alterar C# para: 

#### 3. **RegistroPDF**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `string? (nullable=True)`
- **SQL:** `varchar(100) (NOT NULL)`
- **Correção:** Alterar C# para: 

---

### ⚠️ RepactuacaoAta.cs

**Status:** ⚠️ 15 discrepância(s) encontrada(s)

#### 1. **AtaId**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `Guid (nullable=False)`
- **SQL:** `uniqueidentifier (NULL)`
- **Correção:** Alterar C# para: ?

#### 2. **AtaRegistroPrecos**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public AtaRegistroPrecos AtaRegistroPrecos`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 3. **NumeroAta**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? NumeroAta`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 4. **AnoAta**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? AnoAta`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 5. **AnoProcesso**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int? AnoProcesso`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 6. **NumeroProcesso**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string NumeroProcesso`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 7. **Objeto**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string Objeto`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 8. **DataInicio**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public DateTime? DataInicio`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 9. **DataFim**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public DateTime? DataFim`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 10. **Status**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public bool Status`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 11. **FornecedorId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid FornecedorId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 12. **ItemVeiculoAtaId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid ItemVeiculoAtaId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 13. **NumItem**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int? NumItem`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 14. **Quantidade**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int? Quantidade`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 15. **ValorUnitario**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public double? ValorUnitario`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

---

### ⚠️ RepactuacaoContrato.cs

**Status:** ⚠️ 42 discrepância(s) encontrada(s)

#### 1. **ContratoId**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `Guid (nullable=False)`
- **SQL:** `uniqueidentifier (NULL)`
- **Correção:** Alterar C# para: ?

#### 2. **Contrato**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Contrato Contrato`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 3. **NumeroContrato**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? NumeroContrato`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 4. **AnoContrato**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? AnoContrato`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 5. **AnoProcesso**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int? AnoProcesso`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 6. **NumeroProcesso**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? NumeroProcesso`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 7. **Objeto**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? Objeto`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 8. **TipoContrato**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? TipoContrato`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 9. **DataInicio**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public DateTime? DataInicio`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 10. **DataFim**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public DateTime? DataFim`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 11. **ContratoEncarregados**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public bool ContratoEncarregados`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 12. **ContratoOperadores**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public bool ContratoOperadores`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 13. **ContratoMotoristas**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public bool ContratoMotoristas`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 14. **ContratoLavadores**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public bool ContratoLavadores`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 15. **CustoMensalEncarregado**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public double? CustoMensalEncarregado`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 16. **CustoMensalOperador**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public double? CustoMensalOperador`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 17. **CustoMensalMotorista**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public double? CustoMensalMotorista`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 18. **CustoMensalLavador**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public double? CustoMensalLavador`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 19. **QuantidadeEncarregado**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int? QuantidadeEncarregado`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 20. **QuantidadeMotorista**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int? QuantidadeMotorista`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 21. **QuantidadeOperador**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int? QuantidadeOperador`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 22. **QuantidadeLavador**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int? QuantidadeLavador`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 23. **Status**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public bool Status`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 24. **FornecedorId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid FornecedorId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 25. **NotaFiscalId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid NotaFiscalId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 26. **Ano**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int Ano`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 27. **Mes**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int Mes`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 28. **AtualizaContrato**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public bool AtualizaContrato`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 29. **ItemVeiculoId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid ItemVeiculoId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 30. **NumItem**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int? NumItem`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 31. **Quantidade**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int? Quantidade`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 32. **ValorUnitario**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public double? ValorUnitario`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 33. **RepactuacaoTerceirizacaoId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid RepactuacaoTerceirizacaoId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 34. **ValorEncarregado**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public double? ValorEncarregado`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 35. **ValorOperador**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public double? ValorOperador`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 36. **ValorMotorista**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public double? ValorMotorista`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 37. **ValorLavador**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public double? ValorLavador`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 38. **QtdEncarregados**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int? QtdEncarregados`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 39. **QtdOperadores**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int? QtdOperadores`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 40. **QtdMotoristas**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int? QtdMotoristas`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 41. **QtdLavadores**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int? QtdLavadores`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 42. **RepactuacaoServicoId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid RepactuacaoServicoId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

---

### ⚠️ RepactuacaoServicos.cs

**Status:** ⚠️ 45 discrepância(s) encontrada(s)

#### 1. **ContratoId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid ContratoId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 2. **Contrato**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Contrato Contrato`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 3. **NumeroContrato**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? NumeroContrato`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 4. **AnoContrato**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? AnoContrato`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 5. **Vigencia**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int? Vigencia`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 6. **Prorrogacao**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int? Prorrogacao`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 7. **AnoProcesso**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int? AnoProcesso`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 8. **NumeroProcesso**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? NumeroProcesso`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 9. **Objeto**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? Objeto`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 10. **TipoContrato**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? TipoContrato`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 11. **DataInicio**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public DateTime? DataInicio`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 12. **DataFim**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public DateTime? DataFim`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 13. **ContratoEncarregados**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public bool ContratoEncarregados`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 14. **ContratoOperadores**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public bool ContratoOperadores`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 15. **ContratoMotoristas**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public bool ContratoMotoristas`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 16. **ContratoLavadores**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public bool ContratoLavadores`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 17. **CustoMensalEncarregado**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public double? CustoMensalEncarregado`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 18. **CustoMensalOperador**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public double? CustoMensalOperador`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 19. **CustoMensalMotorista**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public double? CustoMensalMotorista`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 20. **CustoMensalLavador**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public double? CustoMensalLavador`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 21. **QuantidadeEncarregado**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int? QuantidadeEncarregado`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 22. **QuantidadeMotorista**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int? QuantidadeMotorista`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 23. **QuantidadeOperador**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int? QuantidadeOperador`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 24. **QuantidadeLavador**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int? QuantidadeLavador`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 25. **Status**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public bool Status`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 26. **FornecedorId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid FornecedorId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 27. **NotaFiscalId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid NotaFiscalId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 28. **Ano**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int Ano`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 29. **Mes**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int Mes`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 30. **Descricao**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? Descricao`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 31. **Percentual**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public double? Percentual`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 32. **AtualizaContrato**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public bool AtualizaContrato`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 33. **ItemVeiculoId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid ItemVeiculoId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 34. **NumItem**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int? NumItem`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 35. **Quantidade**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int? Quantidade`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 36. **ValorUnitario**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public double? ValorUnitario`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 37. **RepactuacaoTerceirizacaoId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid RepactuacaoTerceirizacaoId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 38. **ValorEncarregado**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public double? ValorEncarregado`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 39. **ValorOperador**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public double? ValorOperador`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 40. **ValorMotorista**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public double? ValorMotorista`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 41. **ValorLavador**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public double? ValorLavador`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 42. **QtdEncarregados**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int? QtdEncarregados`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 43. **QtdOperadores**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int? QtdOperadores`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 44. **QtdMotoristas**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int? QtdMotoristas`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 45. **QtdLavadores**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int? QtdLavadores`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

---

### ⚠️ RepactuacaoTerceirizacao.cs

**Status:** ⚠️ 38 discrepância(s) encontrada(s)

#### 1. **ContratoId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid ContratoId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 2. **Contrato**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Contrato Contrato`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 3. **NumeroContrato**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? NumeroContrato`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 4. **AnoContrato**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? AnoContrato`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 5. **Vigencia**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int? Vigencia`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 6. **Prorrogacao**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int? Prorrogacao`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 7. **AnoProcesso**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int? AnoProcesso`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 8. **NumeroProcesso**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? NumeroProcesso`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 9. **Objeto**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? Objeto`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 10. **TipoContrato**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? TipoContrato`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 11. **DataInicio**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public DateTime? DataInicio`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 12. **DataFim**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public DateTime? DataFim`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 13. **Valor**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public double? Valor`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 14. **ContratoEncarregados**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public bool ContratoEncarregados`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 15. **ContratoOperadores**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public bool ContratoOperadores`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 16. **ContratoMotoristas**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public bool ContratoMotoristas`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 17. **ContratoLavadores**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public bool ContratoLavadores`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 18. **CustoMensalEncarregado**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public double? CustoMensalEncarregado`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 19. **CustoMensalOperador**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public double? CustoMensalOperador`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 20. **CustoMensalMotorista**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public double? CustoMensalMotorista`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 21. **CustoMensalLavador**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public double? CustoMensalLavador`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 22. **QuantidadeEncarregado**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int? QuantidadeEncarregado`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 23. **QuantidadeMotorista**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int? QuantidadeMotorista`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 24. **QuantidadeOperador**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int? QuantidadeOperador`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 25. **QuantidadeLavador**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int? QuantidadeLavador`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 26. **Status**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public bool Status`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 27. **FornecedorId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid FornecedorId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 28. **NotaFiscalId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid NotaFiscalId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 29. **Ano**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int Ano`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 30. **Mes**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int Mes`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 31. **Descricao**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? Descricao`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 32. **Percentual**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public double? Percentual`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 33. **AtualizaContrato**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public bool AtualizaContrato`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 34. **ItemVeiculoId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid ItemVeiculoId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 35. **NumItem**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int? NumItem`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 36. **Quantidade**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int? Quantidade`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 37. **ValorUnitario**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public double? ValorUnitario`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 38. **RepactuacaoServicoId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid RepactuacaoServicoId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

---

### ⚠️ Requisitante.cs

**Status:** ⚠️ 6 discrepância(s) encontrada(s)

#### 1. **Requisitante**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Requisitante? Requisitante`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 2. **Nome**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `string? (nullable=True)`
- **SQL:** `varchar(100) (NOT NULL)`
- **Correção:** Alterar C# para: 

#### 3. **Ponto**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `string? (nullable=True)`
- **SQL:** `varchar(50) (NOT NULL)`
- **Correção:** Alterar C# para: 

#### 4. **Status**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `bool (nullable=False)`
- **SQL:** `bit (NULL)`
- **Correção:** Alterar C# para: ?

#### 5. **UsuarioIdAlteracao**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `string? (nullable=True)`
- **SQL:** `nvarchar(50) (NOT NULL)`
- **Correção:** Alterar C# para: 

#### 6. **SetorSolicitanteId**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `Guid (nullable=False)`
- **SQL:** `uniqueidentifier (NULL)`
- **Correção:** Alterar C# para: ?

---

### ⚠️ SecaoPatrimonial.cs

**Status:** ⚠️ 1 discrepância(s) encontrada(s)

#### 1. **NomeSecao**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `string? (nullable=True)`
- **SQL:** `varchar(50) (NOT NULL)`
- **Correção:** Alterar C# para: 

---

### ⚠️ SetorPatrimonial.cs

**Status:** ⚠️ 3 discrepância(s) encontrada(s)

#### 1. **NomeSetor**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `string? (nullable=True)`
- **SQL:** `varchar(50) (NOT NULL)`
- **Correção:** Alterar C# para: 

#### 2. **DetentorId**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `string? (nullable=True)`
- **SQL:** `nvarchar(450) (NOT NULL)`
- **Correção:** Alterar C# para: 

#### 3. **SetorBaixa**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `bool (nullable=False)`
- **SQL:** `bit (NULL)`
- **Correção:** Alterar C# para: ?

---

### ⚠️ SetorSolicitante.cs

**Status:** ⚠️ 4 discrepância(s) encontrada(s)

#### 1. **SetorSolicitante**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public SetorSolicitante? SetorSolicitante`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 2. **NomeUsuarioAlteracao**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? NomeUsuarioAlteracao`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 3. **Nome**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `string? (nullable=True)`
- **SQL:** `varchar(200) (NOT NULL)`
- **Correção:** Alterar C# para: 

#### 4. **Status**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `bool (nullable=False)`
- **SQL:** `bit (NULL)`
- **Correção:** Alterar C# para: ?

---

### ⚠️ TipoServico.cs

**Status:** ⚠️ 34 discrepância(s) encontrada(s)

#### 1. **NomeServico**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `string? (nullable=True)`
- **SQL:** `nvarchar(100) (NOT NULL)`
- **Correção:** Alterar C# para: 

#### 2. **Descricao**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `string (nullable=False)`
- **SQL:** `nvarchar(500) (NULL)`
- **Correção:** Alterar C# para: ?

#### 3. **TurnoId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid TurnoId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 4. **NomeTurno**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? NomeTurno`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 5. **HoraInicio**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public TimeSpan HoraInicio`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 6. **HoraFim**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public TimeSpan HoraFim`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 7. **AssociacaoId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid? AssociacaoId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 8. **MotoristaId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid MotoristaId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 9. **VeiculoId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid? VeiculoId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 10. **DataInicio**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public DateTime DataInicio`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 11. **DataFim**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public DateTime DataFim`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 12. **Observacoes**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? Observacoes`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 13. **EscalaDiaId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid EscalaDiaId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 14. **DataEscala**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public DateTime DataEscala`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 15. **HoraIntervaloInicio**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public TimeSpan? HoraIntervaloInicio`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 16. **HoraIntervaloFim**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public TimeSpan? HoraIntervaloFim`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 17. **Lotacao**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? Lotacao`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 18. **NumeroSaidas**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int NumeroSaidas`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 19. **StatusMotorista**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string StatusMotorista`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 20. **RequisitanteId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid? RequisitanteId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 21. **FolgaId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid FolgaId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 22. **Tipo**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string Tipo`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 23. **FeriasId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid FeriasId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 24. **MotoristaSubId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid? MotoristaSubId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 25. **CoberturaId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid CoberturaId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 26. **MotoristaFolgaId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid MotoristaFolgaId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 27. **MotoristaCoberturaId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid MotoristaCoberturaId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 28. **Motivo**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? Motivo`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 29. **StatusOriginal**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? StatusOriginal`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 30. **ObservacaoId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid ObservacaoId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 31. **Titulo**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? Titulo`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 32. **Prioridade**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string Prioridade`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 33. **ExibirDe**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public DateTime ExibirDe`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 34. **ExibirAte**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public DateTime ExibirAte`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

---

### ⚠️ Turno.cs

**Status:** ⚠️ 32 discrepância(s) encontrada(s)

#### 1. **TipoServicoId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid TipoServicoId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 2. **NomeServico**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? NomeServico`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 3. **Descricao**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string Descricao`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 4. **NomeTurno**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `string? (nullable=True)`
- **SQL:** `nvarchar(50) (NOT NULL)`
- **Correção:** Alterar C# para: 

#### 5. **AssociacaoId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid? AssociacaoId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 6. **MotoristaId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid MotoristaId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 7. **VeiculoId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid? VeiculoId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 8. **DataInicio**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public DateTime DataInicio`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 9. **DataFim**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public DateTime DataFim`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 10. **Observacoes**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? Observacoes`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 11. **EscalaDiaId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid EscalaDiaId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 12. **DataEscala**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public DateTime DataEscala`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 13. **HoraIntervaloInicio**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public TimeSpan? HoraIntervaloInicio`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 14. **HoraIntervaloFim**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public TimeSpan? HoraIntervaloFim`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 15. **Lotacao**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? Lotacao`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 16. **NumeroSaidas**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int NumeroSaidas`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 17. **StatusMotorista**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string StatusMotorista`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 18. **RequisitanteId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid? RequisitanteId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 19. **FolgaId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid FolgaId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 20. **Tipo**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string Tipo`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 21. **FeriasId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid FeriasId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 22. **MotoristaSubId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid? MotoristaSubId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 23. **CoberturaId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid CoberturaId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 24. **MotoristaFolgaId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid MotoristaFolgaId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 25. **MotoristaCoberturaId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid MotoristaCoberturaId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 26. **Motivo**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? Motivo`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 27. **StatusOriginal**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? StatusOriginal`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 28. **ObservacaoId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid ObservacaoId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 29. **Titulo**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? Titulo`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 30. **Prioridade**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string Prioridade`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 31. **ExibirDe**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public DateTime ExibirDe`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 32. **ExibirAte**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public DateTime ExibirAte`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

---

### ⚠️ Unidade.cs

**Status:** ⚠️ 2 discrepância(s) encontrada(s)

#### 1. **Descricao**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `string? (nullable=True)`
- **SQL:** `varchar(100) (NOT NULL)`
- **Correção:** Alterar C# para: 

#### 2. **Status**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `bool (nullable=False)`
- **SQL:** `bit (NULL)`
- **Correção:** Alterar C# para: ?

---

### ⚠️ VAssociado.cs

**Status:** ⚠️ 31 discrepância(s) encontrada(s)

#### 1. **TipoServicoId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid TipoServicoId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 2. **NomeServico**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? NomeServico`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 3. **Descricao**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string Descricao`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 4. **TurnoId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid TurnoId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 5. **NomeTurno**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? NomeTurno`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 6. **HoraInicio**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public TimeSpan HoraInicio`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 7. **HoraFim**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public TimeSpan HoraFim`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 8. **AssociacaoId**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `Guid? (nullable=True)`
- **SQL:** `uniqueidentifier (NOT NULL)`
- **Correção:** Alterar C# para: 

#### 9. **DataFim**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `DateTime (nullable=False)`
- **SQL:** `date (NULL)`
- **Correção:** Alterar C# para: ?

#### 10. **EscalaDiaId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid EscalaDiaId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 11. **DataEscala**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public DateTime DataEscala`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 12. **HoraIntervaloInicio**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public TimeSpan? HoraIntervaloInicio`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 13. **HoraIntervaloFim**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public TimeSpan? HoraIntervaloFim`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 14. **Lotacao**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? Lotacao`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 15. **NumeroSaidas**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int NumeroSaidas`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 16. **StatusMotorista**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string StatusMotorista`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 17. **RequisitanteId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid? RequisitanteId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 18. **FolgaId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid FolgaId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 19. **Tipo**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string Tipo`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 20. **FeriasId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid FeriasId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 21. **MotoristaSubId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid? MotoristaSubId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 22. **CoberturaId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid CoberturaId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 23. **MotoristaFolgaId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid MotoristaFolgaId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 24. **MotoristaCoberturaId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid MotoristaCoberturaId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 25. **Motivo**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? Motivo`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 26. **StatusOriginal**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? StatusOriginal`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 27. **ObservacaoId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid ObservacaoId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 28. **Titulo**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? Titulo`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 29. **Prioridade**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string Prioridade`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 30. **ExibirDe**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public DateTime ExibirDe`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 31. **ExibirAte**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public DateTime ExibirAte`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

---

### ⚠️ Veiculo.cs

**Status:** ⚠️ 10 discrepância(s) encontrada(s)

#### 1. **Veiculo**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Veiculo? Veiculo`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 2. **NomeUsuarioAlteracao**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? NomeUsuarioAlteracao`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 3. **Status**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `bool (nullable=False)`
- **SQL:** `bit (NULL)`
- **Correção:** Alterar C# para: ?

#### 4. **Reserva**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `bool (nullable=False)`
- **SQL:** `bit (NULL)`
- **Correção:** Alterar C# para: ?

#### 5. **Economildo**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `bool (nullable=False)`
- **SQL:** `bit (NULL)`
- **Correção:** Alterar C# para: ?

#### 6. **VeiculoProprio**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `bool (nullable=False)`
- **SQL:** `bit (NULL)`
- **Correção:** Alterar C# para: ?

#### 7. **Placa**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `string? (nullable=True)`
- **SQL:** `varchar(10) (NOT NULL)`
- **Correção:** Alterar C# para: 

#### 8. **DataAlteracao**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `DateTime? (nullable=True)`
- **SQL:** `datetime (NOT NULL)`
- **Correção:** Alterar C# para: 

#### 9. **MarcaId**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `Guid? (nullable=True)`
- **SQL:** `uniqueidentifier (NOT NULL)`
- **Correção:** Alterar C# para: 

#### 10. **ModeloId**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `Guid? (nullable=True)`
- **SQL:** `uniqueidentifier (NOT NULL)`
- **Correção:** Alterar C# para: 

---

### ⚠️ VeiculoAta.cs

**Status:** ⚠️ 1 discrepância(s) encontrada(s)

#### 1. **VeiculoAta**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public VeiculoAta? VeiculoAta`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

---

### ⚠️ VeiculoContrato.cs

**Status:** ⚠️ 1 discrepância(s) encontrada(s)

#### 1. **VeiculoContrato**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public VeiculoContrato? VeiculoContrato`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

---

### ⚠️ VeiculoPadraoViagem.cs

**Status:** ⚠️ 4 discrepância(s) encontrada(s)

#### 1. **TotalViagens**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public int TotalViagens`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 2. **MediaDuracaoMinutos**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public decimal? MediaDuracaoMinutos`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 3. **MediaKmPorViagem**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public decimal? MediaKmPorViagem`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 4. **MediaKmPorDia**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public decimal? MediaKmPorDia`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

---

### ⚠️ Viagem.cs

**Status:** ⚠️ 25 discrepância(s) encontrada(s)

#### 1. **CriarViagemFechada**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public bool CriarViagemFechada`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 2. **EditarAPartirData**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public DateTime? EditarAPartirData`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 3. **KmAtual**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `int? (nullable=True)`
- **SQL:** `int (NOT NULL)`
- **Correção:** Alterar C# para: 

#### 4. **OperacaoBemSucedida**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public bool? OperacaoBemSucedida`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 5. **ArquivoFoto**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public IFormFile? ArquivoFoto`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 6. **HoraInicial**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public DateTime? HoraInicial`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 7. **SuporteIntegro**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public bool? SuporteIntegro`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 8. **SuporteDefeituoso**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public bool? SuporteDefeituoso`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 9. **Resumo**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? Resumo`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 10. **Data**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? Data`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 11. **Hora**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? Hora`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 12. **Viagem**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Viagem? Viagem`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 13. **StatusCartaoAbastecimento**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? StatusCartaoAbastecimento`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 14. **StatusCartaoAbastecimentoFinal**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? StatusCartaoAbastecimentoFinal`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 15. **StatusDocumento**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? StatusDocumento`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 16. **StatusDocumentoFinal**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? StatusDocumentoFinal`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 17. **ArlaEntregue**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public bool? ArlaEntregue`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 18. **ArlaDevolvido**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public bool? ArlaDevolvido`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 19. **CaboEntregue**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public bool? CaboEntregue`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 20. **CaboDevolvido**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public bool? CaboDevolvido`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 21. **HoraFinalizacao**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? HoraFinalizacao`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 22. **NomeUsuarioAgendamento**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? NomeUsuarioAgendamento`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 23. **NomeUsuarioCancelamento**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? NomeUsuarioCancelamento`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 24. **NomeUsuarioCriacao**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? NomeUsuarioCriacao`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 25. **NomeUsuarioFinalizacao**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public string? NomeUsuarioFinalizacao`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

---

### ⚠️ ViagemEstatistica.cs

**Status:** ⚠️ 11 discrepância(s) encontrada(s)

#### 1. **Id**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `int (nullable=False)`
- **SQL:** `int (NULL)`
- **Correção:** Alterar C# para: ?

#### 2. **ViagensPorStatusJson**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `string (nullable=False)`
- **SQL:** `nvarchar(max) (NULL)`
- **Correção:** Alterar C# para: ?

#### 3. **ViagensPorMotoristaJson**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `string (nullable=False)`
- **SQL:** `nvarchar(max) (NULL)`
- **Correção:** Alterar C# para: ?

#### 4. **ViagensPorVeiculoJson**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `string (nullable=False)`
- **SQL:** `nvarchar(max) (NULL)`
- **Correção:** Alterar C# para: ?

#### 5. **ViagensPorFinalidadeJson**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `string (nullable=False)`
- **SQL:** `nvarchar(max) (NULL)`
- **Correção:** Alterar C# para: ?

#### 6. **ViagensPorRequisitanteJson**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `string (nullable=False)`
- **SQL:** `nvarchar(max) (NULL)`
- **Correção:** Alterar C# para: ?

#### 7. **ViagensPorSetorJson**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `string (nullable=False)`
- **SQL:** `nvarchar(max) (NULL)`
- **Correção:** Alterar C# para: ?

#### 8. **CustosPorMotoristaJson**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `string (nullable=False)`
- **SQL:** `nvarchar(max) (NULL)`
- **Correção:** Alterar C# para: ?

#### 9. **CustosPorVeiculoJson**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `string (nullable=False)`
- **SQL:** `nvarchar(max) (NULL)`
- **Correção:** Alterar C# para: ?

#### 10. **KmPorVeiculoJson**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `string (nullable=False)`
- **SQL:** `nvarchar(max) (NULL)`
- **Correção:** Alterar C# para: ?

#### 11. **CustosPorTipoJson**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `string (nullable=False)`
- **SQL:** `nvarchar(max) (NULL)`
- **Correção:** Alterar C# para: ?

---

### ⚠️ ViagensEconomildo.cs

**Status:** ⚠️ 3 discrepância(s) encontrada(s)

#### 1. **ViagemEconomildoId**

- **Problema:** Coluna ausente no SQL
- **Severidade:** 🔵 INFO
- **C#:** `public Guid ViagemEconomildoId`
- **SQL:** `(não existe no banco)`
- **Correção:** Adicionar coluna ao banco ou marcar com [NotMapped]

#### 2. **VeiculoId**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `Guid (nullable=False)`
- **SQL:** `uniqueidentifier (NULL)`
- **Correção:** Alterar C# para: ?

#### 3. **MotoristaId**

- **Problema:** Nullable incompatível
- **Severidade:** 🔴 CRÍTICO
- **C#:** `Guid (nullable=False)`
- **SQL:** `uniqueidentifier (NULL)`
- **Correção:** Alterar C# para: ?

---

