/* ╔════════════════════════════════════════════════════════════════════════════════════════════════════╗
   ║ 🚀 ARQUIVO: MultaUploadController.cs                                                                ║
   ║ 📂 CAMINHO: /Controllers                                                                            ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 🎯 OBJETIVO: Upload de PDFs de multas via Syncfusion Uploader. Validação + normalização de nomes.   ║
   ╠════════════════════════════════════════════════════════════════════════════════════════════════════╣
   ║ 📋 ÍNDICE: Save(), Remove() - salva em wwwroot/DadosEditaveis/Multas/, apenas .pdf aceito           ║
   ║ 🔗 DEPS: Syncfusion Uploader, File System | 📅 28/01/2026 | 👤 Copilot | 📝 v2.0                    ║
   ╚════════════════════════════════════════════════════════════════════════════════════════════════════╝
*/

/****************************************************************************************
 * ⚡ CONTROLLER: MultaUploadController
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Gerenciar upload de PDFs de multas usando Syncfusion EJ2 Uploader
 *                   Validação de formato, normalização de nomes, salvamento em disco
 * 📥 ENTRADAS     : IList<IFormFile> UploadFiles - Arquivos PDF de multas
 * 📤 SAÍDAS       : JSON formato Syncfusion (success/error), nomes de arquivos salvos
 * 🔗 CHAMADA POR  : Syncfusion Uploader (JavaScript) das páginas de multas
 * 🔄 CHAMA        : FrotiX.Services (normalização de nomes), File System
 * 📦 DEPENDÊNCIAS : ASP.NET Core, Syncfusion EJ2 Uploader, FrotiX.Services, File System
 *
 * ⚠️  VALIDAÇÕES:
 *    - Apenas PDFs são aceitos (.pdf)
 *    - Normalização de nomes de arquivo (remove caracteres especiais)
 *    - Verifica se pasta de destino existe (cria se não existir)
 *
 * 📂 DESTINO:
 *    - Arquivos salvos em: wwwroot/DadosEditaveis/Multas/
 ****************************************************************************************/
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
    [Route("api/[controller]")]
    [ApiController]
    public class MultaUploadController :ControllerBase
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        /****************************************************************************************
         * ⚡ FUNÇÃO: MultaUploadController (Construtor)
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Injetar dependências do hosting environment
         ****************************************************************************************/
        public MultaUploadController(IWebHostEnvironment hostingEnvironment)
        {
            try
            {
                _hostingEnvironment = hostingEnvironment;
            }
            catch (Exception error)
            {
                Alerta.TratamentoErroComLinha("MultaUploadController.cs" , "MultaUploadController" , error);
            }
        }

        [HttpPost("Save")]
        public IActionResult Save(IList<IFormFile> UploadFiles)
        {
            try
            {
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

                var uploadedFiles = new List<object>();
                var pastaMultas = Path.Combine(_hostingEnvironment.WebRootPath , "DadosEditaveis" , "Multas");

                if (!Directory.Exists(pastaMultas))
                {
                    Directory.CreateDirectory(pastaMultas);
                }

                foreach (var file in UploadFiles)
                {
                    try
                    {
                        // Validação de extensão
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

                        // Normalização do nome
                        string nomeOriginal = Path.GetFileNameWithoutExtension(file.FileName);
                        string nomeNormalizado = Servicos.TiraAcento(nomeOriginal);

                        // Adiciona timestamp para evitar conflitos
                        string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                        string nomeArquivo = $"{nomeNormalizado}_{timestamp}{extensao}";

                        string caminhoCompleto = Path.Combine(pastaMultas , nomeArquivo);

                        // Salva o arquivo
                        using (var stream = new FileStream(caminhoCompleto , FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }

                        // Adiciona à lista de sucesso
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

                return Ok(new
                {
                    files = uploadedFiles
                });
            }
            catch (Exception error)
            {
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

        [HttpPost("Remove")]
        public IActionResult Remove(IList<IFormFile> UploadFiles)
        {
            try
            {
                if (UploadFiles == null || UploadFiles.Count == 0)
                {
                    // Tenta remover pelo nome enviado via form data
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

                var removedFiles = new List<object>();
                var pastaMultas = Path.Combine(_hostingEnvironment.WebRootPath , "DadosEditaveis" , "Multas");

                foreach (var file in UploadFiles)
                {
                    try
                    {
                        string caminhoCompleto = Path.Combine(pastaMultas , file.FileName);

                        if (System.IO.File.Exists(caminhoCompleto))
                        {
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

                return Ok(new
                {
                    files = removedFiles
                });
            }
            catch (Exception error)
            {
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

        private IActionResult RemoveByFileName(string fileName)
        {
            try
            {
                var pastaMultas = Path.Combine(_hostingEnvironment.WebRootPath , "DadosEditaveis" , "Multas");
                string caminhoCompleto = Path.Combine(pastaMultas , fileName);

                if (System.IO.File.Exists(caminhoCompleto))
                {
                    System.IO.File.Delete(caminhoCompleto);

                    return Ok(new
                    {
                        name = fileName ,
                        status = "Sucesso" ,
                        statusCode = "200"
                    });
                }

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

        [HttpGet("GetFileList")]
        public IActionResult GetFileList()
        {
            try
            {
                var pastaMultas = Path.Combine(_hostingEnvironment.WebRootPath , "DadosEditaveis" , "Multas");

                if (!Directory.Exists(pastaMultas))
                {
                    return Ok(new
                    {
                        files = new List<object>()
                    });
                }

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

                return Ok(new
                {
                    files = files
                });
            }
            catch (Exception error)
            {
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

        [HttpPost("Chunk")]
        public IActionResult Chunk(IList<IFormFile> chunkFile , string fileName , string chunkIndex)
        {
            try
            {
                var pastaMultas = Path.Combine(_hostingEnvironment.WebRootPath , "DadosEditaveis" , "Multas");
                var tempPath = Path.Combine(pastaMultas , "temp");

                if (!Directory.Exists(tempPath))
                {
                    Directory.CreateDirectory(tempPath);
                }

                var file = chunkFile[0];
                var chunkPath = Path.Combine(tempPath , $"{fileName}.part_{chunkIndex}");

                using (var stream = new FileStream(chunkPath , FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                return Ok(new
                {
                    chunkIndex = chunkIndex ,
                    status = "Sucesso"
                });
            }
            catch (Exception error)
            {
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

        [HttpPost("MergeChunks")]
        public IActionResult MergeChunks(string fileName , string totalChunks)
        {
            try
            {
                var pastaMultas = Path.Combine(_hostingEnvironment.WebRootPath , "DadosEditaveis" , "Multas");
                var tempPath = Path.Combine(pastaMultas , "temp");

                string nomeOriginal = Path.GetFileNameWithoutExtension(fileName);
                string extensao = Path.GetExtension(fileName);
                string nomeNormalizado = Servicos.TiraAcento(nomeOriginal);
                string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                string nomeArquivoFinal = $"{nomeNormalizado}_{timestamp}{extensao}";

                var finalPath = Path.Combine(pastaMultas , nomeArquivoFinal);

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
                            System.IO.File.Delete(chunkPath);
                        }
                    }
                }

                return Ok(new
                {
                    name = nomeArquivoFinal ,
                    status = "Sucesso" ,
                    originalName = fileName
                });
            }
            catch (Exception error)
            {
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
