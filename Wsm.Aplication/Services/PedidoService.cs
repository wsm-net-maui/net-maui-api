using Wsm.Aplication.DTOs.Movimentacoes;
using Wsm.Aplication.DTOs.Pedidos;
using Wsm.Aplication.Interfaces;
using Wsm.Aplication.Mappers;
using Wsm.Domain.Entities;
using Wsm.Domain.Enums;
using Wsm.Domain.Interfaces;
using Wsm.Domain.Interfaces.Repositories;

namespace Wsm.Aplication.Services;

public class PedidoService : IPedidoService
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IProdutoRepository _produtoRepository;
    private readonly IMovimentacaoEstoqueRepository _movimentacaoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PedidoService(
        IPedidoRepository pedidoRepository,
        IProdutoRepository produtoRepository,
        IMovimentacaoEstoqueRepository movimentacaoRepository,
        IUnitOfWork unitOfWork)
    {
        _pedidoRepository = pedidoRepository;
        _produtoRepository = produtoRepository;
        _movimentacaoRepository = movimentacaoRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<PedidoResponseDto>> ObterTodosAsync()
    {
        var pedidos = await _pedidoRepository.ObterTodosAsync();
        return pedidos.Select(PedidoMapper.ToResponseDto);
    }

    public async Task<PedidoResponseDto?> ObterPorIdAsync(Guid id)
    {
        var pedido = await _pedidoRepository.ObterPorIdAsync(id);
        return pedido is null ? null : PedidoMapper.ToResponseDto(pedido);
    }

    public async Task<IEnumerable<PedidoResponseDto>> ObterPorUsuarioAsync(Guid usuarioId)
    {
        var pedidos = await _pedidoRepository.ObterPorUsuarioAsync(usuarioId);
        return pedidos.Select(PedidoMapper.ToResponseDto);
    }

    public async Task<PedidoResponseDto> CriarAsync(CriarPedidoRequestDto request, Guid usuarioId)
    {
        if (usuarioId == Guid.Empty)
            throw new UnauthorizedAccessException("Usuário não autenticado");

        if (request.Itens.Count == 0)
            throw new ArgumentException("Pedido deve ter ao menos um item");

        var pedido = new Pedido(usuarioId);

        foreach (var item in request.Itens)
        {
            var produto = await _produtoRepository.ObterPorIdAsync(item.ProdutoId)
                ?? throw new KeyNotFoundException($"Produto não encontrado: {item.ProdutoId}");

            if (!produto.Ativo)
                throw new InvalidOperationException($"Produto inativo: {produto.Nome}");

            pedido.AdicionarItem(produto.Id, produto.Nome, item.Quantidade, produto.PrecoVenda);
        }

        await _pedidoRepository.AdicionarAsync(pedido);
        await _unitOfWork.CommitAsync();

        var pedidoCompleto = await _pedidoRepository.ObterPorIdAsync(pedido.Id);
        return PedidoMapper.ToResponseDto(pedidoCompleto!);
    }

    public async Task<PedidoResponseDto> ConcluirAsync(Guid id, Guid usuarioId)
    {
        if (usuarioId == Guid.Empty)
            throw new UnauthorizedAccessException("Usuário não autenticado");

        var pedido = await _pedidoRepository.ObterPorIdAsync(id)
            ?? throw new KeyNotFoundException("Pedido não encontrado");

        foreach (var item in pedido.Itens)
        {
            var produto = await _produtoRepository.ObterPorIdAsync(item.ProdutoId)
                ?? throw new KeyNotFoundException($"Produto não encontrado: {item.ProdutoId}");

            produto.RemoverEstoque(item.Quantidade);
            _produtoRepository.Atualizar(produto);

            var movimentacao = new MovimentacaoEstoque(
                produtoId: produto.Id,
                tipo: TipoMovimentacao.Saida,
                quantidade: item.Quantidade,
                usuarioId: usuarioId,
                observacao: $"Saída por conclusão do pedido {pedido.Numero}");

            await _movimentacaoRepository.AdicionarAsync(movimentacao);
        }

        pedido.Concluir();
        _pedidoRepository.Atualizar(pedido);

        await _unitOfWork.CommitAsync();

        var pedidoAtualizado = await _pedidoRepository.ObterPorIdAsync(id);
        return PedidoMapper.ToResponseDto(pedidoAtualizado!);
    }

    public async Task CancelarAsync(Guid id)
    {
        var pedido = await _pedidoRepository.ObterPorIdAsync(id)
            ?? throw new KeyNotFoundException("Pedido não encontrado");

        pedido.Cancelar();
        _pedidoRepository.Atualizar(pedido);
        await _unitOfWork.CommitAsync();
    }
}
