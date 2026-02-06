using FrotiX.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FrotiX.Controllers
{
    /// ╔══════════════════════════════════════════════════════════════════════════════╗
    /// ║ 📌 NOME: MultaUploadController                                             ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 📝 DESCRIÇÃO:                                                                ║
    /// ║    Subsistema de recepção e auditoria de autos de infração (Multas).         ║
    /// ╠══════════════════════════════════════════════════════════════════════════════╣
    /// ║ 🔗 ESCOPO: API REST                                                          ║
    /// ║    • Rota base: /api/MultaUpload                                            ║
    /// ╚══════════════════════════════════════════════════════════════════════════════╝
    [Route("api/[controller]")]
    [ApiController]
    public class MultaUploadController : ControllerBase
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ILogService _log;

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: MultaUploadController (Construtor)                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Inicializa o controlador de upload.                                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • hostingEnvironment (IWebHostEnvironment): Ambiente de hospedagem.      ║
        /// ║    • logService (ILogService): Serviço de log centralizado.                  ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        public MultaUploadController(IWebHostEnvironment hostingEnvironment , ILogService logService)
        {
            try
            {
                _hostingEnvironment = hostingEnvironment;
                _log = logService;
            }
            catch (Exception error)
            {
                _log.Error("MultaUploadController.Constructor", error );
                Alerta.TratamentoErroComLinha("MultaUploadController.cs" , "MultaUploadController" , error);
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Save (POST)                                                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Recebe arquivos do uploader, normaliza nomes e salva no servidor.        ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • UploadFiles (IList<IFormFile>): Arquivos enviados.                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com status dos uploads.                             ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpPost("Save")]
        public IActionResult Save(IList<IFormFile> UploadFiles)
        {
            try
            {
                // [VALIDACAO] Verifica se há arquivos enviados.
                if (UploadFiles == null || UploadFiles.Count == 0)
                {
                    return Ok(new
                    {
                        error = new
                        {
                            code = "400" ,
                            message = "Nenhum arquivo foi enviado"
                        }
                    });
                }

                // [DADOS] Prepara lista de retorno e pasta de destino.
                var uploadedFiles = new List<object>();
                var pastaMultas = Path.Combine(_hostingEnvironment.WebRootPath , "DadosEditaveis" , "Multas");

                if (!Directory.Exists(pastaMultas))
                {
                    // [ARQUIVO] Cria pasta se não existir.
                    Directory.CreateDirectory(pastaMultas);
                }

                foreach (var file in UploadFiles)
                {
                    try
                    {
                        // [VALIDACAO] Validação de extensão.
                        string extensao = Path.GetExtension(file.FileName).ToLower();
                        if (extensao != ".pdf")
                        {
                            uploadedFiles.Add(new
                            {
                                name = file.FileName ,
                                size = file.Length ,
                                status = "Falha" ,
                                statusCode = "400" ,
                                error = "Apenas arquivos PDF são permitidos"
                            });
                            continue;
                        }

                        // [NORMALIZACAO] Normalização do nome.
                        string nomeOriginal = Path.GetFileNameWithoutExtension(file.FileName);
                        string nomeNormalizado = Servicos.TiraAcento(nomeOriginal);

                        // [NORMALIZACAO] Adiciona timestamp para evitar conflitos.
                        string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                        string nomeArquivo = $"{nomeNormalizado}_{timestamp}{extensao}";

                        string caminhoCompleto = Path.Combine(pastaMultas , nomeArquivo);

                        // [ARQUIVO] Salva o arquivo.
                        using (var stream = new FileStream(caminhoCompleto , FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }

                        // [RETORNO] Adiciona à lista de sucesso.
                        uploadedFiles.Add(new
                        {
                            name = nomeArquivo ,
                            size = file.Length ,
                            status = "Sucesso" ,
                            statusCode = "200" ,
                            type = extensao.Replace("." , "") ,
                            validationMessages = new
                            {
                            } ,
                            originalName = file.FileName
                        });
                    }
                    catch (Exception fileError)
                    {
                        // [ERRO] Trata falha por arquivo.
                        _log.Error("MultaUploadController.Save.ForEach", fileError );
                        Alerta.TratamentoErroComLinha("MultaUploadController.cs" , "Save.ForEach" , fileError);
                        uploadedFiles.Add(new
                        {
                            name = file.FileName ,
                            size = file.Length ,
                            status = "Falha" ,
                            statusCode = "500" ,
                            error = $"Erro ao salvar arquivo: {fileError.Message}"
                        });
                    }
                }

                // [RETORNO] Lista de uploads processados.
                return Ok(new
                {
                    files = uploadedFiles
                });
            }
            catch (Exception error)
            {
                _log.Error("MultaUploadController.Save", error );
                Alerta.TratamentoErroComLinha("MultaUploadController.cs" , "Save" , error);
                return Ok(new
                {
                    error = new
                    {
                        code = "500" ,
                        message = "Erro ao processar upload: " + error.Message
                    }
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Remove (POST)                                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Remove o arquivo físico do servidor.                                     ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • UploadFiles (IList<IFormFile>): Arquivos enviados.                      ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com status de remoção.                              ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpPost("Remove")]
        public IActionResult Remove(IList<IFormFile> UploadFiles)
        {
            try
            {
                // [VALIDACAO] Verifica payload.
                if (UploadFiles == null || UploadFiles.Count == 0)
                {
                    // [REGRA] Tenta remover pelo nome enviado via form data.
                    var fileName = Request.Form["fileName"].FirstOrDefault();
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        return RemoveByFileName(fileName);
                    }

                    return Ok(new
                    {
                        error = new
                        {
                            code = "400" ,
                            message = "Nenhum arquivo especificado para remoção"
                        }
                    });
                }

                // [DADOS] Prepara lista de retorno e pasta de destino.
                var removedFiles = new List<object>();
                var pastaMultas = Path.Combine(_hostingEnvironment.WebRootPath , "DadosEditaveis" , "Multas");

                foreach (var file in UploadFiles)
                {
                    try
                    {
                        // [ARQUIVO] Resolve caminho do arquivo.
                        string caminhoCompleto = Path.Combine(pastaMultas , file.FileName);

                        if (System.IO.File.Exists(caminhoCompleto))
                        {
                            // [ARQUIVO] Remove arquivo.
                            System.IO.File.Delete(caminhoCompleto);

                            removedFiles.Add(new
                            {
                                name = file.FileName ,
                                status = "Sucesso" ,
                                statusCode = "200"
                            });
                        }
                        else
                        {
                            // [RETORNO] Arquivo não encontrado.
                            removedFiles.Add(new
                            {
                                name = file.FileName ,
                                status = "Falha" ,
                                statusCode = "404" ,
                                error = "Arquivo não encontrado"
                            });
                        }
                    }
                    catch (Exception fileError)
                    {
                        // [ERRO] Trata falha por arquivo.
                        _log.Error("MultaUploadController.Remove.ForEach", fileError );
                        Alerta.TratamentoErroComLinha("MultaUploadController.cs" , "Remove.ForEach" , fileError);
                        removedFiles.Add(new
                        {
                            name = file.FileName ,
                            status = "Falha" ,
                            statusCode = "500" ,
                            error = $"Erro ao remover arquivo: {fileError.Message}"
                        });
                    }
                }

                // [RETORNO] Lista de remoções processadas.
                return Ok(new
                {
                    files = removedFiles
                });
            }
            catch (Exception error)
            {
                _log.Error("MultaUploadController.Remove", error );
                Alerta.TratamentoErroComLinha("MultaUploadController.cs" , "Remove" , error);
                return Ok(new
                {
                    error = new
                    {
                        code = "500" ,
                        message = "Erro ao processar remoção: " + error.Message
                    }
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: RemoveByFileName                                                 ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Remove arquivo físico pelo nome informado.                               ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        private IActionResult RemoveByFileName(string fileName)
        {
            try
            {
                // [ARQUIVO] Resolve caminho completo.
                var pastaMultas = Path.Combine(_hostingEnvironment.WebRootPath , "DadosEditaveis" , "Multas");
                string caminhoCompleto = Path.Combine(pastaMultas , fileName);

                if (System.IO.File.Exists(caminhoCompleto))
                {
                    // [ARQUIVO] Remove arquivo.
                    System.IO.File.Delete(caminhoCompleto);

                    return Ok(new
                    {
                        name = fileName ,
                        status = "Sucesso" ,
                        statusCode = "200"
                    });
                }

                // [RETORNO] Arquivo não encontrado.
                return Ok(new
                {
                    error = new
                    {
                        code = "404" ,
                        message = "Arquivo não encontrado"
                    }
                });
            }
            catch (Exception error)
            {
                _log.Error("MultaUploadController.RemoveByFileName", error );
                Alerta.TratamentoErroComLinha("MultaUploadController.cs" , "RemoveByFileName" , error);
                return Ok(new
                {
                    error = new
                    {
                        code = "500" ,
                        message = "Erro ao remover arquivo: " + error.Message
                    }
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: GetFileList (GET)                                               ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Lista arquivos disponíveis na pasta de multas.                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com arquivos e metadados.                           ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpGet("GetFileList")]
        public IActionResult GetFileList()
        {
            try
            {
            // [DADOS] Resolve pasta de multas.
                var pastaMultas = Path.Combine(_hostingEnvironment.WebRootPath , "DadosEditaveis" , "Multas");

                if (!Directory.Exists(pastaMultas))
                {
                    // [RETORNO] Nenhum arquivo encontrado.
                    return Ok(new
                    {
                        files = new List<object>()
                    });
                }

                // [DADOS] Lista arquivos e metadados.
                var files = Directory.GetFiles(pastaMultas)
                    .Select(filePath => new FileInfo(filePath))
                    .Select(fileInfo => new
                    {
                        name = fileInfo.Name ,
                        size = fileInfo.Length ,
                        type = fileInfo.Extension.Replace("." , "") ,
                        dateModified = fileInfo.LastWriteTime.ToString("dd/MM/yyyy HH:mm:ss")
                    })
                    .ToList();

                // [RETORNO] Lista de arquivos.
                return Ok(new
                {
                    files = files
                });
            }
            catch (Exception error)
            {
                _log.Error("MultaUploadController.GetFileList", error );
                Alerta.TratamentoErroComLinha("MultaUploadController.cs" , "GetFileList" , error);
                return Ok(new
                {
                    error = new
                    {
                        code = "500" ,
                        message = "Erro ao listar arquivos: " + error.Message
                    }
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: Chunk (POST)                                                    ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Recebe um chunk de upload e salva temporariamente.                       ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • chunkFile (IList<IFormFile>): Parte do arquivo.                         ║
        /// ║    • fileName (string): Nome do arquivo original.                            ║
        /// ║    • chunkIndex (string): Índice do chunk.                                   ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com status do chunk.                                ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpPost("Chunk")]
        public IActionResult Chunk(IList<IFormFile> chunkFile , string fileName , string chunkIndex)
        {
            try
            {
            // [ARQUIVO] Resolve pasta e caminho temporário.
                var pastaMultas = Path.Combine(_hostingEnvironment.WebRootPath , "DadosEditaveis" , "Multas");
                var tempPath = Path.Combine(pastaMultas , "temp");

                if (!Directory.Exists(tempPath))
                {
                    Directory.CreateDirectory(tempPath);
                }

                // [ARQUIVO] Salva chunk.
                var file = chunkFile[0];
                var chunkPath = Path.Combine(tempPath , $"{fileName}.part_{chunkIndex}");

                using (var stream = new FileStream(chunkPath , FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                // [RETORNO] Confirma chunk salvo.
                return Ok(new
                {
                    chunkIndex = chunkIndex ,
                    status = "Sucesso"
                });
            }
            catch (Exception error)
            {
                _log.Error("MultaUploadController.Chunk", error );
                Alerta.TratamentoErroComLinha("MultaUploadController.cs" , "Chunk" , error);
                return Ok(new
                {
                    error = new
                    {
                        code = "500" ,
                        message = "Erro ao processar chunk: " + error.Message
                    }
                });
            }
        }

        /// ╔══════════════════════════════════════════════════════════════════════════════╗
        /// ║ 📌 NOME: MergeChunks (POST)                                             ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📝 DESCRIÇÃO:                                                                ║
        /// ║    Mescla chunks temporários em um arquivo final.                           ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📥 PARÂMETROS:                                                               ║
        /// ║    • fileName (string): Nome original do arquivo.                            ║
        /// ║    • totalChunks (string): Total de chunks.                                  ║
        /// ╠══════════════════════════════════════════════════════════════════════════════╣
        /// ║ 📤 RETORNO:                                                                  ║
        /// ║    • IActionResult: JSON com status do merge.                                ║
        /// ╚══════════════════════════════════════════════════════════════════════════════╝
        [HttpPost("MergeChunks")]
        public IActionResult MergeChunks(string fileName , string totalChunks)
        {
            try
            {
            // [ARQUIVO] Resolve pastas e nome final.
                var pastaMultas = Path.Combine(_hostingEnvironment.WebRootPath , "DadosEditaveis" , "Multas");
                var tempPath = Path.Combine(pastaMultas , "temp");

                string nomeOriginal = Path.GetFileNameWithoutExtension(fileName);
                string extensao = Path.GetExtension(fileName);
                string nomeNormalizado = Servicos.TiraAcento(nomeOriginal);
                string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                string nomeArquivoFinal = $"{nomeNormalizado}_{timestamp}{extensao}";

                var finalPath = Path.Combine(pastaMultas , nomeArquivoFinal);

                // [ARQUIVO] Mescla chunks em arquivo final.
                using (var finalStream = new FileStream(finalPath , FileMode.Create))
                {
                    for (int i = 0; i < int.Parse(totalChunks); i++)
                    {
                        var chunkPath = Path.Combine(tempPath , $"{fileName}.part_{i}");
                        if (System.IO.File.Exists(chunkPath))
                        {
                            using (var chunkStream = new FileStream(chunkPath , FileMode.Open))
                            {
                                chunkStream.CopyTo(finalStream);
                            }
                            // [ARQUIVO] Remove chunk após merge.
                            System.IO.File.Delete(chunkPath);
                        }
                    }
                }

                // [RETORNO] Arquivo final pronto.
                return Ok(new
                {
                    name = nomeArquivoFinal ,
                    status = "Sucesso" ,
                    originalName = fileName
                });
            }
            catch (Exception error)
            {
                _log.Error("MultaUploadController.MergeChunks", error );
                Alerta.TratamentoErroComLinha("MultaUploadController.cs" , "MergeChunks" , error);
                return Ok(new
                {
                    error = new
                    {
                        code = "500" ,
                        message = "Erro ao mesclar chunks: " + error.Message
                    }
                });
            }
        }
    }
}
