using System;
using System.Collections.Generic;
using System.Linq;

namespace NipahData_Tokenizer
{
	public interface IProgressiveList<T>
	{
		int Pointer {get;}
		void Reset();
		void ResetBegin();
		void ResetAll();
		bool HasNext();
		T This();
		T Next();
		T Back();
		T Look_Next();
		T Look_Next(int plus);
		T Look_Back();
		void PointerNext();
		void PointerBack();

		void List(List<T> list);
		void CopyList(List<T> list);
		void Add(T item);
		void Remove(T item);


		T Next(Predicate<T> ignoreThis);
		T Next(List<T> toAdd);
		T NextUntil(Predicate<T> match);

		ProgressiveListInstance<T> Instantiate();
		ProgressiveListInstance<T> MakeInstance(Predicate<T> @while);
		List<T> List();

		bool ClosureCheck(Predicate<T> open, Predicate<T> close, int startsWith = 0);
	}
}
