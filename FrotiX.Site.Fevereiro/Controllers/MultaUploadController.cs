/* ****************************************************************************************
 * ⚡ ARQUIVO: MultaUploadController.cs
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Gerenciar upload e remoção de PDFs de multas via Syncfusion Uploader,
 *                   com validação de extensão e normalização de nomes.
 *
 * 📥 ENTRADAS     : IList<IFormFile> UploadFiles.
 *
 * 📤 SAÍDAS       : JSON de sucesso/erro compatível com Syncfusion.
 *
 * 🔗 CHAMADA POR  : Syncfusion Uploader nas páginas de multas.
 *
 * 🔄 CHAMA        : Servicos.TiraAcento(), File System.
 *
 * 📦 DEPENDÊNCIAS : ASP.NET Core, Syncfusion EJ2 Uploader, FrotiX.Services.
 *
 * 📂 DESTINO      : wwwroot/DadosEditaveis/Multas/
 **************************************************************************************** */

/****************************************************************************************
 * ⚡ CONTROLLER: MultaUploadController
 * --------------------------------------------------------------------------------------
 * 🎯 OBJETIVO     : Expor endpoints de upload e remoção de PDFs de multas.
 *
 * 📥 ENTRADAS     : Arquivos enviados pelo uploader.
 *
 * 📤 SAÍDAS       : JSON com status individual de cada arquivo.
 *
 * 🔗 CHAMADA POR  : Frontend (Syncfusion Uploader).
 *
 * 🔄 CHAMA        : File IO e utilitários de normalização.
 *
 * 📦 DEPENDÊNCIAS : ASP.NET Core, IWebHostEnvironment, FrotiX.Services.
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
         * 🎯 OBJETIVO     : Injetar dependência de ambiente para acesso ao wwwroot.
         *
         * 📥 ENTRADAS     : [IWebHostEnvironment] hostingEnvironment.
         *
         * 📤 SAÍDAS       : Instância configurada.
         *
         * 🔗 CHAMADA POR  : ASP.NET Core DI.
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

        /****************************************************************************************
         * ⚡ FUNÇÃO: Save
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Salvar arquivos PDF de multa no diretório configurado.
         *
         * 📥 ENTRADAS     : UploadFiles (lista de arquivos).
         *
         * 📤 SAÍDAS       : JSON com status e nomes salvos.
         *
         * 🔗 CHAMADA POR  : Syncfusion Uploader (upload).
         *
         * 🔄 CHAMA        : Servicos.TiraAcento(), FileStream, Directory.CreateDirectory().
         ****************************************************************************************/
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

        /****************************************************************************************
         * ⚡ FUNÇÃO: Remove
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Remover arquivo PDF enviado anteriormente.
         *
         * 📥 ENTRADAS     : UploadFiles ou nome via Request.Form["fileName"].
         *
         * 📤 SAÍDAS       : JSON com status de remoção.
         *
         * 🔗 CHAMADA POR  : Syncfusion Uploader (remove).
         *
         * 🔄 CHAMA        : File.Delete(), Directory/Path.
         ****************************************************************************************/
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

        /****************************************************************************************
         * ⚡ FUNÇÃO: GetFileList
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Listar arquivos de multas existentes no diretório.
         *
         * 📥 ENTRADAS     : Nenhuma.
         *
         * 📤 SAÍDAS       : JSON com lista de arquivos e metadados.
         *
         * 🔗 CHAMADA POR  : Tela/controle de arquivos de multas.
         *
         * 🔄 CHAMA        : Directory.GetFiles(), FileInfo.
         ****************************************************************************************/
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

        /****************************************************************************************
         * ⚡ FUNÇÃO: Chunk
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Receber e armazenar partes (chunks) de arquivo.
         *
         * 📥 ENTRADAS     : chunkFile, fileName, chunkIndex.
         *
         * 📤 SAÍDAS       : JSON com status do chunk.
         *
         * 🔗 CHAMADA POR  : Uploader em modo chunked.
         *
         * 🔄 CHAMA        : FileStream, Directory.CreateDirectory().
         ****************************************************************************************/
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

        /****************************************************************************************
         * ⚡ FUNÇÃO: MergeChunks
         * --------------------------------------------------------------------------------------
         * 🎯 OBJETIVO     : Mesclar chunks em um único arquivo final.
         *
         * 📥 ENTRADAS     : fileName, totalChunks.
         *
         * 📤 SAÍDAS       : JSON com nome final e status.
         *
         * 🔗 CHAMADA POR  : Uploader após envio completo.
         *
         * 🔄 CHAMA        : FileStream, Servicos.TiraAcento().
         ****************************************************************************************/
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
