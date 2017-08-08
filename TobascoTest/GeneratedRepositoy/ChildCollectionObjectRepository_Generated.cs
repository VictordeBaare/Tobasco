using System;
using TobascoTest.GeneratedEntity;
using Tobasco;
using TobascoTest.IGenerateRepository;
using System.Collections.Generic;

namespace TobascoTest.GeneratedRepositoy
{
	
	public  partial class ChildCollectionObjectRepository : IChildCollectionObjectRepository
	{
		private IGenericRepository<ChildCollectionObject> _genericRepository;
public ChildCollectionObjectRepository(IGenericRepository<ChildCollectionObject> genericRepository)
{
	_genericRepository = genericRepository;
	
}
		
		
		
		public ChildCollectionObject Save(ChildCollectionObject  childcollectionobject)
{
	
	
	childcollectionobject = _genericRepository.Save(childcollectionobject);
	
	
	
	return childcollectionobject;
}
public ChildCollectionObject GetById(long id)
        {
            return _genericRepository.GetById(id);
        }
	
	}
}