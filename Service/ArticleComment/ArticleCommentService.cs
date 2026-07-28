using Core;
using Core.Article.Dto;
using Core.Article.Entity;
using Repository;
using System.Collections.Immutable;
using System.Net;

namespace Service
{
    public class ArticleCommentService : IArticleCommentService
    {
        private readonly IArticleCommentRepository _articleCommentRepository;
        private readonly IArticleRepository _articleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ArticleCommentService(
            IArticleCommentRepository articleCommentRepository,
            IArticleRepository articleRepository,
            IUnitOfWork unitOfWork)
        {
            _articleCommentRepository = articleCommentRepository;
            _articleRepository = articleRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseModelDto<IImmutableList<ArticleCommentDto>>> GetByArticleId(Guid articleId)
        {
            var comments = await _articleCommentRepository.GetByArticleId(articleId);

            // Tek sorgu ile gelen düz liste bellekte ağaca dönüştürülür; tek seviye
            // olduğu için ParentCommentId -> cevaplar araması yeterli.
            var repliesByParent = comments
                .Where(c => c.ParentCommentId.HasValue)
                .ToLookup(c => c.ParentCommentId!.Value);

            var tree = comments
                .Where(c => !c.ParentCommentId.HasValue)
                .Select(root => ToDto(root, repliesByParent[root.Id].Select(r => ToDto(r, [])).ToList()))
                .ToImmutableList();

            return ResponseModelDto<IImmutableList<ArticleCommentDto>>.Success(tree);
        }

        public async Task<ResponseModelDto<Guid>> Add(Guid articleId, CreateArticleCommentDto createArticleCommentDto)
        {
            if (createArticleCommentDto.ParentCommentId.HasValue)
            {
                var parent = await _articleCommentRepository.GetByArticleIdAndCommentId(
                    articleId, createArticleCommentDto.ParentCommentId.Value);

                if (parent is null)
                {
                    return ResponseModelDto<Guid>.Failure(
                        "Cevap verilen yorum bu makalede bulunamadı.", HttpStatusCode.NotFound);
                }

                // Tek seviye kuralı: cevaba cevap verilemez.
                if (parent.ParentCommentId.HasValue)
                {
                    return ResponseModelDto<Guid>.Failure("Bir cevaba cevap yazılamaz.");
                }
            }

            var entity = new ArticleCommentEntity
            {
                ArticleId = articleId,
                ParentCommentId = createArticleCommentDto.ParentCommentId,
                Content = createArticleCommentDto.Content,
                GuestName = createArticleCommentDto.GuestName,
                GuestEmail = createArticleCommentDto.GuestEmail
            };

            await _articleCommentRepository.Add(entity);
            await _unitOfWork.CommitAsync();

            await _articleRepository.IncrementCommentCount(articleId);

            return ResponseModelDto<Guid>.Success(entity.Id, HttpStatusCode.Created);
        }

        public async Task<ResponseModelDto<NoContent>> Remove(Guid articleId, Guid commentId)
        {
            var comment = await _articleCommentRepository.GetByArticleIdAndCommentId(articleId, commentId);
            if (comment is null)
            {
                return ResponseModelDto<NoContent>.Failure(
                    $"Comment with id {commentId} not found for article {articleId}.", HttpStatusCode.NotFound);
            }

            // Kök yorum silinince cevapları da silinir; aksi halde query filter kökü gizler
            // ve cevaplar yetim kalarak listede hiç görünmez ama sayaçta durmaya devam ederdi.
            var replies = comment.ParentCommentId.HasValue
                ? []
                : await _articleCommentRepository.GetReplies(comment.Id);

            foreach (var reply in replies)
            {
                await _articleCommentRepository.Remove(reply);
            }
            await _articleCommentRepository.Remove(comment);
            await _unitOfWork.CommitAsync();

            await _articleRepository.DecrementCommentCount(articleId, replies.Count + 1);

            return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
        }

        private static ArticleCommentDto ToDto(ArticleCommentEntity entity, IReadOnlyList<ArticleCommentDto> replies) =>
            new(entity.Id, entity.GuestName, entity.Content, entity.CreatedDate.ToShortDateString(), replies);
    }
}
